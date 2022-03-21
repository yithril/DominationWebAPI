using AutoWrapper.Wrappers;
using Domination_WebAPI.Data;
using Domination_WebAPI.Domain.Interface;
using Domination_WebAPI.Enum;
using Domination_WebAPI.Models;
using Domination_WebAPI.Settings;
using Domination_WebAPI.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Domination_WebAPI.Domain
{
    public class PlayerResourceDomain : IPlayerResourceDomain
    {
        private readonly ApplicationDbContext _context;
        private readonly GameSettings _settings;

        public PlayerResourceDomain(ApplicationDbContext dbContext, GameSettings settings)
        {
            _context = dbContext;
            _settings = settings;
        }

        //Gather resources from individual nodes
        public async Task<ApiResponse> UpdatePlayerResources(int UserId)
        {
            var user = await _context.Users.Where(x => x.Id == UserId)
                .Include(y => y.Bonus)
                .ThenInclude(z => z.Bonus)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new ApiResponse(500, "User not found");
            }

            List<GameZone> gameZones = await (from zone in _context.GameZones
                                              join gameNode in _context.GameNodes on zone.Id equals gameNode.GameZoneId
                                              where gameNode.UserId == UserId 
                                              select zone)
                                       .Include(x => x.Nodes)
                                       .ThenInclude(y => y.NodeImprovement)
                                       .ToListAsync();

            //Check if each node is set to contribute resources
            var gameNodes = gameZones.SelectMany(z => z.Nodes).ToList()
                .Where(x => GetNodeTicks(x) > 0 && x.CurrentResourceType != ResourceTypeEnum.None && x.CurrentResourceType != ResourceTypeEnum.Residential && x.IsDevelopable && x.IsActive).ToList();

            var currentBonuses = user.Bonus.Where(x => x.CreatedDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow).Select(x => x.Bonus).ToList();

            foreach (var n in gameNodes)
            {
                var bonuses = currentBonuses.Where(x => x.Resource == n.CurrentResourceType).ToList();
                int amount = GetNodeResources(n, bonuses);
                user = IncrementPlayerResource(user, n.CurrentResourceType, amount);
            }

            //update the population, people need to eat!
            UpdatePlayerPopulation(user, currentBonuses.Where(x => x.Resource == ResourceTypeEnum.Residential).ToList());

            //User cultural effect
            UpdateNodeCulture(user, gameNodes);

            //update the user
            await _context.SaveChangesAsync();

            return new ApiResponse(true);
        }

        public async Task<ApiResponse> SetNodeType(int nodeId, ResourceTypeEnum type, int userId)
        {
            try
            {
                var node = await _context.GameNodes.FindAsync(nodeId);

                var user = await _context.Users.FindAsync(userId);

                if (node == null || user == null)
                {
                    return new ApiResponse("Node not found", 500);
                }

                if (!node.IsDevelopable)
                {
                    return new ApiResponse("Cannot develop.", 200);
                }

                if(node.CheckNodeCost() > user.AcquisitionPoints)
                {
                    return new ApiResponse("Not enouch resources", 200);
                }

                node.CurrentResourceType = type;

                user.AcquisitionPoints -= node.CheckNodeCost();

                await _context.SaveChangesAsync();

                return new ApiResponse(node);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse> SetNodeImprovement(int id, int improvementId, int userId)
        {
            try
            {
                var node = await _context.GameNodes.FindAsync(id);

                var improvement = await _context.NodeImprovement.Where(x => x.Id == improvementId)
                    .Include(y => y.Costs)
                    .FirstOrDefaultAsync();

                var user = await _context.Users.FindAsync(userId);

                if (node == null || improvement == null || user == null)
                {
                    return new ApiResponse("Could not perform action, try again later.", 500);
                }

                int canPay = 0;

                foreach(var c in improvement.Costs)
                {
                    if(CheckResourceCost(user, c.Cost, c.Type))
                    {
                        canPay++;
                    }
                }

                if(canPay != improvement.Costs.Count)
                {
                    return new ApiResponse(Enum.ApiError.NOT_ENOUGH_RESOURCES.ToString(), 200);
                }

                foreach(var c in improvement.Costs)
                {
                    user = IncrementPlayerResource(user, c.Type, c.Cost * -1);
                }

                //Node can only have 1 improvement
                node.NodeImprovementId = improvement.Id;

                await _context.SaveChangesAsync();

                return new ApiResponse();
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse> ResearchTech(int userId, int techId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                var research = await _context.ResearchTech.FindAsync(techId);

                if (user == null || research == null)
                {
                    return new ApiResponse("There was an error try again", 500);
                }

                if (research.ResearchCost > user.Research)
                {
                    return new ApiResponse(Enum.ApiError.NOT_ENOUGH_TECH.ToString(), 200);
                }

                user.Research -= research.ResearchCost;

                var pr = new PlayerResearchTech()
                {
                    UserId = user.Id,
                    ResearchTechId = research.Id
                };

                _context.Add(pr);

                await _context.SaveChangesAsync();

                return new ApiResponse(user);
            }
            catch(Exception ex)
            {
                return new ApiResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse> GetPlayerCurrentResourceOutput(int userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId)
                .Include(y => y.Bonus)
                .ThenInclude(z => z.Bonus)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new ApiResponse("Could not find user", 500);
            }

            //Get all of the nodes connected to the user
            List<GameNode> gameNodes = await _context.GameNodes
                .Where(x => x.UserId == user.Id)
                .Include(y => y.NodeImprovement)
                .ToListAsync();

            var bonuses = user.Bonus.Where(x => x.CreatedDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow).Select(x => x.Bonus).ToList();

            PlayerResourceDTO dto = new PlayerResourceDTO();

            dto.Bonuses = bonuses;
            dto.NationName = user.NationName;
            dto.Money = SumNodes(gameNodes.Where(x => x.CurrentResourceType == ResourceTypeEnum.Commercial).ToList(), bonuses.Where(x => x.Resource == ResourceTypeEnum.Commercial).ToList());
            dto.IndustrialGoods= SumNodes(gameNodes.Where(x => x.CurrentResourceType == ResourceTypeEnum.Industrial).ToList(), bonuses.Where(x => x.Resource == ResourceTypeEnum.Industrial).ToList());
            dto.MilitaryMight = SumNodes(gameNodes.Where(x => x.CurrentResourceType == ResourceTypeEnum.Military).ToList(), bonuses.Where(x => x.Resource == ResourceTypeEnum.Military).ToList());
            dto.Research = SumNodes(gameNodes.Where(x => x.CurrentResourceType == ResourceTypeEnum.Research).ToList(), bonuses.Where(x => x.Resource == ResourceTypeEnum.Research).ToList());
            dto.Population = SumNodes(gameNodes.Where(x => x.CurrentResourceType == ResourceTypeEnum.Residential).ToList(), bonuses.Where(x => x.Resource == ResourceTypeEnum.Residential).ToList());
            dto.Culture = SumNodes(gameNodes.Where(x => x.CurrentResourceType == ResourceTypeEnum.Cultural).ToList(), bonuses.Where(x => x.Resource == ResourceTypeEnum.Cultural).ToList());
            dto.Food = SumNodes(gameNodes.Where(x => x.CurrentResourceType == ResourceTypeEnum.Agricultural).ToList(), bonuses.Where(x => x.Resource == ResourceTypeEnum.Agricultural).ToList());

            return new ApiResponse(dto);
        }

        public async Task<ApiResponse> PurchaseAcquisitionPoints(int userId, ResourceTypeEnum resource)
        {
            var user = await _context.Users.Where(x => x.Id == userId)
                .Include(y => y.Bonus)
                .FirstOrDefaultAsync();

            if(user == null)
            {
                return new ApiResponse("Error", 500);
            }

            int cost = resource == ResourceTypeEnum.Cultural ? _settings.CultureAcquisitionCost : _settings.AcquisitionPointCost;

            if(!CheckResourceCost(user, cost, resource))
            {
                return new ApiResponse(Enum.ApiError.NOT_ENOUGH_RESOURCES, 500);
            }

            user.AcquisitionPoints += _settings.AcquisitionGain;

            IncrementPlayerResource(user, resource, cost * -1);

            await _context.SaveChangesAsync();

            return new ApiResponse(true);
        }

        private void UpdatePlayerPopulation(User user, List<Bonus> bonuses)
        {
            //TODO Pop bonus?
            int foodRequirement = user.Population * _settings.PopFoodRequiredModifier;

            user.Food -= foodRequirement;

            if(user.Food > 0)
            {
                user.Population *= _settings.PopFullModifier;
            }
            else
            {
                var modifier = .003 * user.Food + 1;
                user.Population = (int)(modifier * user.Food);
            }
        }

        private void UpdateNodeCulture(User user, List<GameNode> gameNodes)
        {
            //Get the total cultural output of the user
            var output = SumNodes(gameNodes.Where(x => x.CurrentResourceType == ResourceTypeEnum.Cultural).ToList(), user.Bonus.Select(x => x.Bonus).ToList());

            var modifier = output * _settings.CultureNodeModifier;

            gameNodes.ForEach(x => x.Culture += modifier);

            //Can't be over 1
            gameNodes.Where(x => x.Culture > 1).ToList().ForEach(y => y.Culture = 1);
        }

        private bool CheckResourceCost(User user, int cost, ResourceTypeEnum type)
        {
            switch (type)
            {
                case ResourceTypeEnum.Industrial:
                    return user.IndustrialGoods >= cost;
                case ResourceTypeEnum.Commercial:
                    return user.Money >= cost;
                case ResourceTypeEnum.Research:
                    return user.Research >= cost;
                case ResourceTypeEnum.Agricultural:
                    return user.Food >= cost;
                case ResourceTypeEnum.Cultural:
                    return user.Culture >= cost;
                default:
                    return true;
            }
        }
        
        private int GetNodeTicks(GameNode node)
        {
            return (DateTime.Now - node.LastProducedDate).Hours;
        }

        private User IncrementPlayerResource(User user, ResourceTypeEnum type, int amount)
        {
            switch (type)
            {
                case ResourceTypeEnum.Residential:
                    user.Population += amount;
                    return user;
                case ResourceTypeEnum.Industrial:
                    user.IndustrialGoods += amount;
                    return user;
                case ResourceTypeEnum.Commercial:
                    user.Money += amount;
                    return user;
                case ResourceTypeEnum.Research:
                    user.Research += amount;
                    return user;
                case ResourceTypeEnum.Agricultural:
                    user.Food += amount;
                    return user;
                case ResourceTypeEnum.Cultural:
                    user.Culture += amount;
                    return user;
                case ResourceTypeEnum.Military:
                    user.MilitaryMight += amount;
                    return user;
                default:
                    return user;
            }
        }

        private int SumNodes(List<GameNode> nodes, List<Bonus> bonuses)
        {
            int sum = 0;

            foreach (GameNode node in nodes)
            {
                sum += GetNodeResources(node, bonuses);
            }

            return sum;
        }

        private int GetNodeResources(GameNode node, List<Bonus> bonuses)
        {
            double baseRate = (double)node.NodeQuality;

            if(node.NodeImprovement != null)
            {
                baseRate *= node.NodeImprovement.Modifier;
                baseRate += node.NodeImprovement.StaticBonus;
            }

            foreach(var b in bonuses)
            {
                baseRate *= b.Modifier;
            }

            return (int)baseRate;
        }

    }
}

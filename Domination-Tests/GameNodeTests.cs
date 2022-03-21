using Domination_WebAPI.Data;
using Domination_WebAPI.Domain;
using Domination_WebAPI.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Domination_Tests
{
    public class GameNodeTests
    {
        [Fact]
        public async Task CreateGameNodes()
        {
            using (var factory = new TestDbContextFactory())
            {
                using (var context = factory.CreateContext())
                {
                    MapDomain domain = new MapDomain(context);

                    var createZone = await domain.CreateGameZone(1, 1);

                    var nodes = await context.GameNodes.Where(x => x.GameZoneId == 1).ToListAsync();

                    var waterNodes = nodes.Where(x => x.CurrentResourceType == Domination_WebAPI.Enum.ResourceTypeEnum.Water).Count();

                    var wasteNodes = nodes.Where(x => x.CurrentResourceType == Domination_WebAPI.Enum.ResourceTypeEnum.Wasteland).Count();

                    var regNodes = nodes.Where(x => x.CurrentResourceType == Domination_WebAPI.Enum.ResourceTypeEnum.None).Count();

                    nodes.Count.Should().BeGreaterThan(1);
                }
            }
        }

        [Fact]
        public async Task UpdatePlayerResources()
        {
            using (var factory = new TestDbContextFactory())
            {
                using (var context = factory.CreateContext())
                {
                    var user = new User()
                    {
                        Id = 1,
                        Population = 100,
                        Food = 100,
                        IndustrialGoods = 100,
                        Money = 100,
                        Research = 100,
                        Culture = 100,
                        MilitaryMight = 100,
                        AccountId = "blah",
                        NationName = "blah",
                        Adjective = "blah"
                    };

                    var farmImprovement = new NodeImprovement()
                    {
                        Id = 1,
                        Name = "Farm implements",
                        ResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Residential,
                        Modifier = 1.1,
                        StaticBonus = 0,
                        Description = "blah"
                    };

                    var industryImprovement = new NodeImprovement()
                    {
                        Id = 2,
                        Name = "Industry implements",
                        ResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Industrial,
                        Modifier = 1,
                        StaticBonus = 10,
                        Description = "blah"
                    };

                    var gamezone = new GameZone()
                    {
                        Id = 1
                    };

                    context.Add(user);
                    context.Add(farmImprovement);
                    context.Add(industryImprovement);
                    context.Add(gamezone);

                    await context.SaveChangesAsync();

                    var farmNodes = new List<GameNode>()
                    {
                        new GameNode() {  CurrentResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Agricultural, NodeImprovement = farmImprovement, NodeImprovementId = farmImprovement.Id },
                        new GameNode() {  CurrentResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Agricultural, UserId = 1 }
                    };

                    var industryNodes = new List<GameNode>()
                    {
                        new GameNode() {  CurrentResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Industrial, NodeImprovement = industryImprovement, NodeImprovementId = industryImprovement.Id },
                        new GameNode() {  CurrentResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Industrial, UserId = 1 }
                    };

                    farmNodes.ForEach(x =>
                    {
                        x.IsActive = true;
                        x.IsDevelopable = true;
                        x.LastProducedDate = System.DateTime.Now.AddHours(-1);
                        x.GameZone = gamezone;
                        x.GameZoneId = gamezone.Id;
                        x.UserId = 1;
                        x.NodeQuality = Domination_WebAPI.Enum.NodeQuality.Very_Low;
                    });

                    industryNodes.ForEach(x =>
                    {
                        x.IsActive = true;
                        x.IsDevelopable = true;
                        x.LastProducedDate = System.DateTime.Now.AddHours(-1);
                        x.GameZone = gamezone;
                        x.GameZoneId = gamezone.Id;
                        x.UserId = 1;
                        x.NodeQuality = Domination_WebAPI.Enum.NodeQuality.Medium;
                    });

                    context.AddRange(farmNodes);
                    context.AddRange(industryNodes);

                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch(Exception e)
                    {
                        var bob = 10;
                    }


                    PlayerResourceDomain domain = new GameEventDomain(context);

                    var response = await domain.UpdatePlayerResources(1);

                    user = await context.Users.FindAsync(1);

                    user.Should().NotBeNull();
                    user.Food.Should().Be(121);
                    user.IndustrialGoods.Should().Be(170);
                    response.StatusCode.Should().Be(200);
                }
            }
        }

        [Fact]
        public async Task UpdatePlayerResources_WithBonuses()
        {
            using (var factory = new TestDbContextFactory())
            {
                using (var context = factory.CreateContext())
                {
                    var user = new User()
                    {
                        Id = 1,
                        Population = 100,
                        Food = 100,
                        IndustrialGoods = 100,
                        Money = 100,
                        Research = 100,
                        Culture = 100,
                        MilitaryMight = 100,
                        AccountId = "blah",
                        NationName = "blah",
                        Adjective = "blah"
                    };

                    var farmImprovement = new NodeImprovement()
                    {
                        Id = 1,
                        Name = "Farm implements",
                        ResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Residential,
                        Modifier = 1.1,
                        StaticBonus = 0,
                        Description = "blah"
                    };

                    var industryImprovement = new NodeImprovement()
                    {
                        Id = 2,
                        Name = "Industry implements",
                        ResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Industrial,
                        Modifier = 1,
                        StaticBonus = 10,
                        Description = "blah"
                    };

                    var gamezone = new GameZone()
                    {
                        Id = 1
                    };

                    context.Add(user);
                    context.Add(farmImprovement);
                    context.Add(industryImprovement);
                    context.Add(gamezone);

                    await context.SaveChangesAsync();

                    var farmNodes = new List<GameNode>()
                    {
                        new GameNode() {  CurrentResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Agricultural, NodeImprovement = farmImprovement, NodeImprovementId = farmImprovement.Id },
                        new GameNode() {  CurrentResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Agricultural, UserId = 1 }
                    };

                    var industryNodes = new List<GameNode>()
                    {
                        new GameNode() {  CurrentResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Industrial, NodeImprovement = industryImprovement, NodeImprovementId = industryImprovement.Id },
                        new GameNode() {  CurrentResourceType = Domination_WebAPI.Enum.ResourceTypeEnum.Industrial, UserId = 1 }
                    };

                    farmNodes.ForEach(x =>
                    {
                        x.IsActive = true;
                        x.IsDevelopable = true;
                        x.LastProducedDate = System.DateTime.Now.AddHours(-1);
                        x.GameZone = gamezone;
                        x.GameZoneId = gamezone.Id;
                        x.UserId = 1;
                        x.NodeQuality = Domination_WebAPI.Enum.NodeQuality.Very_Low;
                    });

                    industryNodes.ForEach(x =>
                    {
                        x.IsActive = true;
                        x.IsDevelopable = true;
                        x.LastProducedDate = System.DateTime.Now.AddHours(-1);
                        x.GameZone = gamezone;
                        x.GameZoneId = gamezone.Id;
                        x.UserId = 1;
                        x.NodeQuality = Domination_WebAPI.Enum.NodeQuality.Medium;
                    });

                    context.AddRange(farmNodes);
                    context.AddRange(industryNodes);

                    var research = new ResearchTech()
                    {
                        Name = "Blah",
                        Description = "Blah",
                        Id = 1
                    };

                    var bonus = new Bonus()
                    {
                        ResearchTechId = 1,
                        Name = "Blah",
                        Description = "Blah",
                        Resource = Domination_WebAPI.Enum.ResourceTypeEnum.Agricultural,
                        Modifier = 2
                    };

                    context.Add(research);
                    context.Add(bonus);

                    await context.SaveChangesAsync();

                    var pBonus = new PlayerBonus()
                    {
                        UserId = 1,
                        BonusId = 1,
                        CreatedDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddHours(1)
                    };

                    context.Add(pBonus);
                    await context.SaveChangesAsync();

                    PlayerResourceDomain domain = new GameEventDomain(context);

                    var response = await domain.UpdatePlayerResources(1);

                    user = await context.Users.FindAsync(1);

                    user.Should().NotBeNull();
                    user.Food.Should().Be(142);
                    user.IndustrialGoods.Should().Be(170);
                    response.StatusCode.Should().Be(200);
                }
            }
        }
    }
}
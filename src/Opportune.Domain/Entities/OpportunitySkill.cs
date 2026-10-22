namespace Opportune.Domain.Entities;

public class OpportunitySkill
{
    public Guid OpportunityId { get; set; }
    public Opportunity Opportunity { get; set; } = null!;
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
}

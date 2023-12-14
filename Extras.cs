public class ProfessionalStandard {
    public string Content { get; set; }
}

public class ActivityDuration {
    public string ActivityType { get; set; }
    public int Count { get; set; }
    public DateTime NextDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class Subrow {
    public string Description { get; set; }
    public string UnitsCost { get; set; }
    public List<Competence> Competences { get; set; }
    public string Title { get; set; }
    public List<bool> Terms { get; set; }
    public string Index { get; set; }
}

public class EduPlan {
    public Block1 Block1 { get; set; }
}
public class Block1 {
    public List<Subrow> Subrows { get; set; }
}

public class Competence {
    public string Code { get; set; }
}

public class Category {
    public string Title { get; set; }
}

public class Indicator {
    public string Code { get; set; }
    public string Content { get; set; }
}

public class UniversalCompetencyRow {
    public Competence Competence { get; set; }
    public Category Category { get; set; }
    public List<Indicator> Indicators { get; set; }
}

public class Section4 {
    public List<ProfessionalStandard> ProfessionalStandards { get; set; }
    public List<UniversalCompetencyRow> universalCompetencyRows { get; set; }
}

public class Section5 {
    public EduPlan EduPlan { get; set; }

    public calendarPlanTable calendarPlanTable { get; set; }
}

public class calendarPlanTable {
    public List<Course> Courses { get; set; }
}

public class Course {
    public List<string> WeekActivityIds { get; set; }
}


public class Content {
    public Section4 Section4 { get; set; }

    public Section5 Section5 { get; set; }
}

public class EducationalProgram {
    public Content Content { get; set; }
}
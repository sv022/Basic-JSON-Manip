using Newtonsoft.Json;

static string Input(string message){
    Console.WriteLine();
    Console.BackgroundColor = ConsoleColor.DarkGreen;
    Console.Write(message);
    Console.BackgroundColor = ConsoleColor.Black;
    string? value = Console.ReadLine();
    Console.WriteLine();
    return value;
}

static void DisplayProfessionalStandards(EducationalProgram program) {
    Console.WriteLine("Код\tНазвание");
    foreach (var standard in program.Content.Section4.ProfessionalStandards) {
        string[] parts = standard.Content.Split(' ');
        if (parts.Length >= 2) {
            string code = parts[0];
            string content = string.Join(" ", parts.Skip(1));
            Console.WriteLine($"{code}\t{content}");
        }
        else Console.WriteLine($"{standard.Content}");

    }
}

static void DisplayCompetenceInfo(EducationalProgram program, string targetCompetenceCode) {
    Console.WriteLine($"\n\nИнформация для {targetCompetenceCode} компетенции:");
    Console.WriteLine($"Компетенция\t\tСодержание");
    if (program.Content.Section4.universalCompetencyRows != null) {
        foreach (var row in program.Content.Section4.universalCompetencyRows) {
            if (row.Competence != null && row.Competence.Code == targetCompetenceCode) {
                Console.WriteLine($"{row.Competence.Code}\t\t{row.Category.Title}");
                if (row.Indicators != null) {
                    foreach (var indicator in row.Indicators)
                        Console.WriteLine($"{indicator.Code}\t\t{indicator.Content}");
                }
            }
        }
    }
}

static void DisplayDisciplineInfo(EducationalProgram program, string targetDisciplineIndex) {
    var targetDiscipline = program.Content.Section5.EduPlan.Block1.Subrows.FirstOrDefault(d => d.Index == targetDisciplineIndex);

    Console.WriteLine($"\n\nИнформация по {targetDisciplineIndex}:");
    if (targetDiscipline != null) {
        Console.WriteLine($"{targetDiscipline.Index} {targetDiscipline.Title}");
        int indexOfFirstDot = targetDiscipline.Description.IndexOf('.');
        if (indexOfFirstDot != -1) {
            string truncatedDescription = targetDiscipline.Description.Substring(0, indexOfFirstDot + 1);
            Console.WriteLine($"Цель: {truncatedDescription}");
        }
        else Console.WriteLine($"Цель: {targetDiscipline.Description}");

        Console.WriteLine("Компетенции: " + string.Join(", ", targetDiscipline.Competences.Select(competence => competence.Code)));
        Console.WriteLine($"Зачетные единицы: {targetDiscipline.UnitsCost}");
        Console.WriteLine($"Семестры: {string.Join(" ", targetDiscipline.Terms.Select((term, index) => term ? (index + 1).ToString() : ""))}");
    }
    else Console.WriteLine($"Дисциплина с индексом {targetDisciplineIndex} не найдена.");
    
}

static void DisplayDisciplinesInSemester(EducationalProgram program, int targetSemester) {
    if (program.Content.Section5 != null && program.Content.Section5.EduPlan != null && program.Content.Section5.EduPlan.Block1 != null && program.Content.Section5.EduPlan.Block1.Subrows != null) {
        var disciplinesInTargetSemester = program.Content.Section5.EduPlan.Block1.Subrows
            .Where(d => d.Terms != null && d.Terms.Count >= targetSemester && d.Terms[targetSemester - 1])
            .Select(d => new { Index = d.Index, Title = d.Title });
        Console.WriteLine($"\n\nСписок дисциплин для {targetSemester} семестра:");
        if (disciplinesInTargetSemester.Any()) {
            Console.WriteLine("Шифр\t\tНазвание дисциплины");
            foreach (var discipline in disciplinesInTargetSemester)
                Console.WriteLine($"{discipline.Index}\t\t{discipline.Title}");

        }
        else Console.WriteLine($"Дисциплины для семестра {targetSemester} не найдены.");
    }
}

static void DisplayWeekActivityInfo(EducationalProgram program, int targetCourse) {
    List<string> weekActivityIds = program.Content.Section5.calendarPlanTable.Courses[targetCourse - 1].WeekActivityIds;
    List<ActivityDuration> durations = CalculateDurations(weekActivityIds);
    Console.WriteLine($"\n\nГрафик учебного процесса для {targetCourse} курса:");
    foreach (var duration in durations)
        Console.WriteLine($"{duration.ActivityType}: Продолжительность {duration.NextDate} - {duration.EndDate}, Количество {duration.Count}");
}

static List<ActivityDuration> CalculateDurations(List<string> weekActivityIds) {
    List<ActivityDuration> durations = new List<ActivityDuration>();
    string currentActivity = "";
    int currentCount = 0;
    DateTime nextDate = new DateTime(2020, 9, 1);
    foreach (var activity in weekActivityIds) {
        if (activity == currentActivity)
            currentCount++;
        else {
            if (!string.IsNullOrEmpty(currentActivity)) {
                var duration = new ActivityDuration {
                    ActivityType = currentActivity,
                    Count = currentCount,
                    NextDate = nextDate.Month == 9 ? nextDate : nextDate.AddDays(-1)
                };
                duration.EndDate = nextDate.AddDays(currentCount * 7).AddDays(-3);
                durations.Add(duration);
                nextDate = nextDate.AddDays(currentCount * 7);
            }
            currentActivity = activity;
            currentCount = 1;
        }
    }

    if (!string.IsNullOrEmpty(currentActivity)) {
        var duration = new ActivityDuration {
            ActivityType = currentActivity,
            Count = currentCount,
            NextDate = nextDate.AddDays(-1)
        };

        duration.EndDate = nextDate.AddDays(currentCount * 7).AddDays(-3);
        durations.Add(duration);
    }

    return durations;
}

string jsonFilePath = "ProfessionalStandards.json";
string jsonString = File.ReadAllText(jsonFilePath);
EducationalProgram program = JsonConvert.DeserializeObject<EducationalProgram>(jsonString);

string targetCompetenceCode;
string targetDisciplineIndex;
int targetSemester;
int targetCourse;

string cmd;
Console.WriteLine("Добро пожаловать в JSONViewer 0.1!\n\n");
do {
    Console.WriteLine("Выберите действие:\n");
    Console.WriteLine("1 - Информация о профессиональных стандартах");
    Console.WriteLine("2 - Ввести код компетенции");
    Console.WriteLine("3 - Ввести индекс дисциплины");
    Console.WriteLine("4 - Ввести номер семестра");
    Console.WriteLine("5 - Ввести номер курса");
    Console.WriteLine("6 - Завершить программу");
    cmd = Input("Введите команду: ");

    switch (cmd){
        case "1":
            DisplayProfessionalStandards(program);
            break;
        case "2":
            targetCompetenceCode = Input("Введите код компетенции: ");
            DisplayCompetenceInfo(program, targetCompetenceCode);
            break;

        case "3":
            targetDisciplineIndex = Input("Введите индекс дисциплины: ");
            DisplayDisciplineInfo(program, targetDisciplineIndex);
            break;

        case "4":
            if (int.TryParse(Input("Введите номер семестра: "), out int semester)) 
                DisplayDisciplinesInSemester(program, semester);
            else 
                Console.WriteLine("Ошибка ввода.");
            break;

        case "5":
            if (int.TryParse(Input("Введите номер курса: "), out int course))
                DisplayWeekActivityInfo(program, course);
            else
                Console.WriteLine("Ошибка ввода.");
            break;

        default:
            Console.WriteLine("Некорректная опция.");
            break;
    }
} while (cmd != "6");

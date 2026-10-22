using Microsoft.AspNetCore.Identity;
using Opportune.Domain.Entities;
using Opportune.Domain.Enums;

namespace Opportune.Persistence.Seed;

/// <summary>
/// Reference data inserted by the first migration. Identifiers are fixed so the
/// migration is repeatable and other data can refer to them.
/// </summary>
internal static class SeedData
{
    private static Guid Id(string prefix, int n) => new($"{prefix}-0000-0000-0000-{n:D12}");

    public static readonly IdentityRole<Guid>[] Roles =
    [
        Role(1, AppRoles.Volunteer),
        Role(2, AppRoles.Organizer),
        Role(3, AppRoles.Admin)
    ];

    private static IdentityRole<Guid> Role(int n, string name) => new()
    {
        Id = Id("a0000000", n),
        Name = name,
        NormalizedName = name.ToUpperInvariant(),
        ConcurrencyStamp = $"role-seed-{n}"
    };

    public static readonly Category[] Categories =
    [
        Cat(1, "environment", "Ətraf mühit", "Environment", "Экология"),
        Cat(2, "education", "Təhsil", "Education", "Образование"),
        Cat(3, "culture-arts", "Mədəniyyət və incəsənət", "Culture and arts", "Культура и искусство"),
        Cat(4, "social-care", "Sosial dəstək", "Social care", "Социальная помощь"),
        Cat(5, "health", "Sağlamlıq", "Health", "Здоровье"),
        Cat(6, "youth", "Gənclər", "Youth", "Молодёжь"),
        Cat(7, "sports", "İdman", "Sports", "Спорт"),
        Cat(8, "events", "Tədbirlər və konfranslar", "Events and conferences", "Мероприятия и конференции"),
        Cat(9, "animals", "Heyvanların müdafiəsi", "Animal welfare", "Защита животных"),
        Cat(10, "humanitarian", "Humanitar yardım", "Humanitarian aid", "Гуманитарная помощь"),
        Cat(11, "technology", "Texnologiya", "Technology", "Технологии")
    ];

    private static Category Cat(int n, string slug, string az, string en, string ru) =>
        new() { Id = Id("c0000000", n), Slug = slug, NameAz = az, NameEn = en, NameRu = ru };

    public static readonly Skill[] Skills =
    [
        Sk(1, "communication", "Ünsiyyət", "Communication", "Коммуникация"),
        Sk(2, "teamwork", "Komanda işi", "Teamwork", "Работа в команде"),
        Sk(3, "leadership", "Liderlik", "Leadership", "Лидерство"),
        Sk(4, "event-organization", "Tədbir təşkili", "Event organization", "Организация мероприятий"),
        Sk(5, "teaching", "Tədris", "Teaching", "Преподавание"),
        Sk(6, "public-speaking", "Natiqlik", "Public speaking", "Публичные выступления"),
        Sk(7, "photography", "Fotoqrafiya", "Photography", "Фотография"),
        Sk(8, "video-production", "Video istehsalı", "Video production", "Видеопроизводство"),
        Sk(9, "graphic-design", "Qrafik dizayn", "Graphic design", "Графический дизайн"),
        Sk(10, "social-media", "Sosial media", "Social media", "Социальные сети"),
        Sk(11, "writing", "Yazı və redaktə", "Writing and editing", "Написание и редактура"),
        Sk(12, "translation", "Tərcümə", "Translation", "Перевод"),
        Sk(13, "first-aid", "İlkin tibbi yardım", "First aid", "Первая помощь"),
        Sk(14, "fundraising", "Vəsait toplanması", "Fundraising", "Сбор средств"),
        Sk(15, "programming", "Proqramlaşdırma", "Programming", "Программирование"),
        Sk(16, "data-analysis", "Məlumat təhlili", "Data analysis", "Анализ данных"),
        Sk(17, "project-management", "Layihə idarəçiliyi", "Project management", "Управление проектами"),
        Sk(18, "research", "Tədqiqat", "Research", "Исследования"),
        Sk(19, "cooking", "Yemək hazırlama", "Cooking", "Приготовление пищи"),
        Sk(20, "working-with-children", "Uşaqlarla iş", "Working with children", "Работа с детьми")
    ];

    private static Skill Sk(int n, string slug, string az, string en, string ru) =>
        new() { Id = Id("d0000000", n), Slug = slug, NameAz = az, NameEn = en, NameRu = ru };
}

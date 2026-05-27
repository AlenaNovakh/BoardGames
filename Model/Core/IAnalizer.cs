namespace Model.Core
{
    // Второй интерфейс — только для аналитики по каталогу.
    // ПРИНЦИП РАЗДЕЛЕНИЯ ИНТЕРФЕЙСА: не смешиваем операции CRUD с аналитикой.
    public interface IAnalizer
    {
        double AverageMinAmount();      // среднее минимальное кол-во игроков
        double AverageMaxAmount();      // среднее максимальное кол-во игроков
        double MeanAmount();            // средний диапазон (max - min) мест
        double MeanAgeRestriction();    // среднее возрастное ограничение
    }
}
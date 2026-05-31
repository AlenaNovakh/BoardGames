namespace Model.Core
{
    public interface IAnalizer
    {
        double AverageMinAmount();
        double AverageMaxAmount();
        double MeanAmount();
        double MeanAgeRestriction();
    }
}
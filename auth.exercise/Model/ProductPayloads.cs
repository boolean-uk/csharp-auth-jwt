namespace auth.exercise.Model
{
    public record ProductPostPayload(string Name, int Price, int Quantity);

    public record ProductPutPayload(int Price, int Quantity);
}
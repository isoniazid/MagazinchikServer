public interface IModelDTO<MODEL, DTO>
{
    public  DTO ToDTO(MODEL model);

    public  MODEL ToModel(DTO dto);
}
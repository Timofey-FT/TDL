public class CreateNoteViewModel
{
    public int Id { set; get; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<int> SelectedTagIds { get; set; } = new List<int>();
}

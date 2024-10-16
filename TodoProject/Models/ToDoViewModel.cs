namespace TodoProject.Models
{
    public class ToDoViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public IEnumerable<ToDo> ToDos { get; set; } = new List<ToDo>();
    }

}

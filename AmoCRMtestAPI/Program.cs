using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AmoCRM;

namespace AmoCRMtestAPI
{
    class Program
    {
        static int user_id = 3430591;

        static AmoCRMTasks amoCRM = new AmoCRMTasks();

        static void Main(string[] args)
        {
            CancellationToken cancellationToken = new CancellationToken();          

            while (true)
            {
                ShowAllTasks(cancellationToken);
                Console.WriteLine("\n Добавить задачу? (y - да / n - нет)");
                string answer = Console.ReadLine().ToLower();
                if (answer == "y") 
                {
                    Console.WriteLine("Введите текст к задаче:");
                    string text = Console.ReadLine();
                    Item[] items = new[]
                    {
                        new Item()
                        {
                            element_id = 0,
                            element_type = ElementType.Contact,
                            complete_till_at = Helpers.UnixTimestampFromDateTime(DateTime.Now),
                            task_type = TaskType.WriteLetter,
                            text = text,
                            created_at = Helpers.UnixTimestampFromDateTime(DateTime.Now),
                            updated_at = Helpers.UnixTimestampFromDateTime(DateTime.Now),
                            responsible_user_id = user_id,
                            created_by = user_id
                        }
                    };
                    Task.Run(async () => await amoCRM.AddTaskAsync(cancellationToken, items)).GetAwaiter().GetResult();
                    Console.WriteLine("Задача добавлена");
                }
                if (answer == "n") break;   
            }
        }

        private static void ShowAllTasks(CancellationToken ct)
        {
            Console.WriteLine("\n === Задачи ===");

            JObject answer = null;

            Task.Run(async () => answer = await amoCRM.GetAllTasks(ct)).GetAwaiter().GetResult();

            foreach (var item in answer.ToObject<RootObjectGetAllTask>()._embedded.items.Where(i => i.is_completed == false))
            {
                ShowItem(item);
            }
        }

        private static void ShowItem(Item item)
        {
            Console.WriteLine("id: {0} text: {1}", item.id, item.text);
        }
    }
}

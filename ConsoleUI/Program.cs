using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ConsolUI
{
    class Program
    {
        private static readonly HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            // API adresini ayarla
            string baseAddress = "http://localhost:5064/api/";

            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("ToDo Uygulaması");

            while (true)
            {
                Console.WriteLine("\nYapmak istediğiniz işlemi seçin:");
                Console.WriteLine("1. Kayıt Ol");
                Console.WriteLine("2. Giriş Yap");
                Console.WriteLine("3. Çıkış");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        bool registerSuccess = await RegisterUser();
                        if (registerSuccess)
                        {
                            Console.WriteLine("Kayıt başarılı! Giriş yapmanız gerekiyor.");
                            await LoginFlow(); // Kayıt sonrası giriş işlemi
                        }
                        break;
                    case "2":
                        await LoginFlow();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, tekrar deneyin.");
                        break;
                }
            }
        }

        private static async Task<bool> RegisterUser()
        {
            Console.Write("Adınızı giriniz: ");
            var firstName = Console.ReadLine();
            Console.Write("Soyadınızı giriniz: ");
            var lastName = Console.ReadLine();
            Console.Write("Kullanıcı Adınızı Girin: ");
            var username = Console.ReadLine();
            Console.Write("Parolanızı Girin: ");
            var password = Console.ReadLine();

            var newUser = new { Username = username, Password = password };
            var response = await httpClient.PostAsJsonAsync("api/Auth/register", newUser);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Kayıt başarılı!");
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Kayıt başarısız! Hata Kodu: {response.StatusCode} - Mesaj: {errorMessage}");
                return false;
            }
        }

        private static async Task LoginFlow()
        {
            while (true)
            {
                var token = await LoginUser();
                if (token != null)
                {
                    Console.WriteLine("Giriş başarılı!");
                    await ShowTodoMenu(token);
                    return;
                }
                else
                {
                    Console.WriteLine("Giriş başarısız! Tekrar deneyin.");
                    Console.WriteLine("\n1. Tekrar Giriş Yap");
                    Console.WriteLine("2. Ana Menüye Dön");

                    var choice = Console.ReadLine();
                    if (choice == "2")
                    {
                        return; // Ana menüye dön
                    }
                }
            }
        }

        private static async Task<string> LoginUser()
        {
            Console.Write("Kullanıcı Adınızı Girin: ");
            var username = Console.ReadLine();
            Console.Write("Parolanızı Girin: ");
            var password = Console.ReadLine();

            var credentials = new { Username = username, Password = password };
            var response = await httpClient.PostAsJsonAsync("auth/login", credentials);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            return null;
        }

        private static async Task ShowTodoMenu(string token)
        {
            while (true)
            {
                Console.WriteLine("\nYapmak istediğiniz işlemi seçin:");
                Console.WriteLine("1. Görev Ekle");
                Console.WriteLine("2. Görevleri Listele");
                Console.WriteLine("3. Tamamlanan Görevleri Listele");
                Console.WriteLine("4. Bekleyen Görevleri Listele");
                Console.WriteLine("5. Çıkış");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await AddTodoItem(token);
                        break;
                    case "2":
                        await GetTodoItems(token);
                        break;
                    case "3":
                        await GetCompletedTodoItems(token);
                        break;
                    case "4":
                        await GetPendingTodoItems(token);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, tekrar deneyin.");
                        break;
                }
            }
        }

        private static async Task AddTodoItem(string token)
        {
            Console.Write("Görev Adını Girin: ");
            var title = Console.ReadLine();
            var newItem = new { Title = title };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.PostAsJsonAsync("todoitem", newItem);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Görev başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Görev eklenirken hata oluştu.");
            }
        }

        private static async Task GetTodoItems(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync("todoitem");
            if (response.IsSuccessStatusCode)
            {
                var items = await response.Content.ReadFromJsonAsync<List<dynamic>>();
                Console.WriteLine("Görevler:");

                foreach (var item in items)
                {
                    DisplayTodoItem(item);
                }
            }
            else
            {
                Console.WriteLine("Görevleri alırken hata oluştu.");
            }
        }

        private static async Task GetCompletedTodoItems(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync("todoitem/completed");
            if (response.IsSuccessStatusCode)
            {
                var items = await response.Content.ReadFromJsonAsync<List<dynamic>>();
                Console.WriteLine("Tamamlanan Görevler:");

                foreach (var item in items)
                {
                    DisplayTodoItem(item);
                }
            }
            else
            {
                Console.WriteLine("Tamamlanan görevleri alırken hata oluştu.");
            }
        }

        private static async Task GetPendingTodoItems(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync("todoitem/pending");
            if (response.IsSuccessStatusCode)
            {
                var items = await response.Content.ReadFromJsonAsync<List<dynamic>>();
                Console.WriteLine("Bekleyen Görevler:");

                foreach (var item in items)
                {
                    DisplayTodoItem(item);
                }
            }
            else
            {
                Console.WriteLine("Bekleyen görevleri alırken hata oluştu.");
            }
        }

        private static void DisplayTodoItem(dynamic item)
        {
            var completed = item.IsCompleted ? "[Tamamlandı]" : "[Beklemede]";
            var style = item.IsCompleted ? "üzeri çizili" : "normal";
            Console.WriteLine($"{item.Id}. {item.Title} {completed} (Durum: {style})");
        }

        private static async Task EditTodoItem(string token, int id)
        {
            Console.Write("Yeni Görev Adını Girin: ");
            var title = Console.ReadLine();
            var updatedItem = new { Title = title };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.PutAsJsonAsync($"todoitem/{id}", updatedItem);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Görev başarıyla güncellendi.");
            }
            else
            {
                Console.WriteLine("Görev güncellenirken hata oluştu.");
            }
        }

        private static async Task DeleteTodoItem(string token, int id)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.DeleteAsync($"todoitem/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Görev başarıyla silindi.");
            }
            else
            {
                Console.WriteLine("Görev silinirken hata oluştu.");
            }
        }

        private static async Task MarkAsCompleted(string token, int id)
        {
            var completedItem = new { IsCompleted = true };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.PutAsJsonAsync($"todoitem/{id}/complete", completedItem);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Görev tamamlandı olarak işaretlendi.");
            }
            else
            {
                Console.WriteLine("Görev işaretlenirken hata oluştu.");
            }
        }
    }
}
# MusicPlayer
Пояснительная записка (Часть Канаев П.)
class ap
    {
        public List<Songs> audiolist;
        public void parse(string str)
        {
            while (!auth_property.Default.security)
            {
                Thread.Sleep(500);
                string url = "https://api.vk.com/method/audio.search?q=" + str + "&access_token=" + auth_property.Default.token;
                WebRequest request = WebRequest.Create(url);

                try
                {
                    WebResponse resp = request.GetResponse();
                    Stream stream = resp.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string js_a = reader.ReadToEnd();
                    reader.Close();
                    resp.Close();
                    js_a = HttpUtility.HtmlDecode(js_a);

                    JToken token = JToken.Parse(js_a);
                    audiolist = token["response"].Children().Skip(1).Select(c => c.ToObject<Songs>()).ToList();
                }
                catch { }
            }
        }
}
Class ap:
Поле audiolist – список, состоящий из объектов класса  Songs, поля которого отвечают за описание каждой конкретной песни.
Метод parse – метод, позволяющий десериализовать json файл – ответ на запрос к VK API.
WebRequest request – переменная класса WebRequest, отвечающая за запрос.
Запихиваем в трай-кетч на случай исключений.
Создаём поток Stream stream, считываем его StreamReader. 
Получаем js_a переменную в формате json.
Парсим json файл, десериализуем его с помощью лямбда выражения, пропустив первый уровень, получаем список объектов-информации о треке. 
Class auth:
private void auth_Load(object sender, EventArgs e)
        {
            string url = "https://oauth.vk.com/authorize?client_id=5303207&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.45";
            webBrowser1.Navigate(url);

        }
private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
               
                string url = webBrowser1.Url.ToString();
                string l = url.Split('#')[1];
                if (l[0] == 'a')
                {
                    auth_property.Default.token = l.Split('&')[0].Split('=')[1];
                    auth_property.Default.security = true;
                    this.Close();
                    MessageBox.Show("Token:" + auth_property.Default.token );
                }
            }

            catch { }
        }
Создаём браузер для авторизации, изучив то, какую ссылку возвращает API VK, создаём файл настроек auth_property, куда помещаем token и, обозначая то, что авторизация прошла успешно.
Class Songs:
public class Songs 
    {
        public int aid { get; set; }
        public int owner_id { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public string url { get; set; }
        public string lurlcs_id { get; set; }
        public int genre { get; set; }
    }
Как я писал уже раньше, класс Songs ответственен за информацию о каждой песне.

Часть Румовского К.
	Класс mpDataBaseController – синглтон, предоставляющий доступ к коллекции аудио. Основной способ взаимодействия – метод AudioList(), возвращающий список аудиофайлов в виде List<mpAudio>. Может сериализоваться в .xml-файл, индивидуальный для пользователя.
	Класс mpAudio предоставляет собой описание одной композиции. Имеет всего два тега (Artist и Title), так как именно их хранит vk.com, предполагаемый источник большей части коллекции. Является абстрактным, один из трёх потомков выбирается в зависимости от способа хранения аудио (копия в коллекции, ссылка внутри файловой системы, web-ссылка).
	Отдельные методы описаны с помощью XML-документации.

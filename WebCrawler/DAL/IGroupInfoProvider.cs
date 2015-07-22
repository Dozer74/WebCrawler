namespace WebCrawler.DAL
{
    public interface IGroupInfoProvider
    {
        /// <summary>
        /// Проверяет, совпадают ли имена новой группы и сохраненной ранее в базе
        /// </summary>
        bool IsGroupUrlSame(string groupName);

        /// <summary>
        /// Возвращает url сохраненной в базе группы
        /// </summary>
        string GetSavedGroupUrl();

        /// <summary>
        /// Возвращает название сохраненной в базе группы
        /// </summary>
        string GetSavedGroupName();

        /// <summary>
        /// Обновляет параметры сохраненной группы
        /// </summary>
        void UpdateGroupInfo(string groupName, string groupUrl);
    }
}
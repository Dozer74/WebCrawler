using System.Collections.Generic;
using WebCrawler.Models;

namespace WebCrawler.DAL
{
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Добавляет указанную модель в базу данных
        /// </summary>
        void AddRecord(DataModel model);

        /// <summary>
        /// Сохраняет все произведенные изменения
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Очищает базу данных
        /// </summary>
        void Truncate();

        /// <summary>
        /// Возвращает все хранящиеся в базе модели
        /// </summary>
        IEnumerable<DataModel> GetAllRecords();
    }
}
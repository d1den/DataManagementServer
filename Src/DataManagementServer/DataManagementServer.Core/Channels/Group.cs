using System;
using System.Reactive;
using System.Reactive.Linq;
using DataManagementServer.Sdk.Channels;

namespace DataManagementServer.Core.Channels
{
    public class Group
    {
        /// <summary>
        /// Id группы
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Id родительской группы
        /// </summary>
        public Guid ParentId { get; private set; }

        /// <summary>
        /// Название группы
        /// </summary>
        private string Name { get; set; } = "New group";

        /// <summary>
        /// Событие обновления
        /// </summary>
        private event EventHandler<UpdateEventArgs> _UpdateEvent;

        /// <summary>
        /// Уведомитель об обновлении
        /// </summary>
        public IObservable<EventPattern<UpdateEventArgs>> ObservableUpdate { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Group()
        {
            Id = Guid.NewGuid();
            ObservableUpdate = Observable.FromEventPattern<UpdateEventArgs>(
                handler => _UpdateEvent += handler,
                handler => _UpdateEvent -= handler);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model">Модель группы</param>
        /// <exception cref="ArgumentNullException">Ошибка при Null модели</exception>
        public Group(GroupModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }
            Id = model.Id;
            SetFieldsByModel(model);
            ObservableUpdate = Observable.FromEventPattern<UpdateEventArgs>(
                handler => _UpdateEvent += handler,
                handler => _UpdateEvent -= handler);
        }

        /// <summary>
        /// Преобразовать к модели
        /// </summary>
        /// <param name="allFields">Получить все поля или никаких</param>
        /// <returns>Модель группы</returns>
        public GroupModel ToModel(bool allFields = false)
        {
            if (!allFields)
            {
                return new GroupModel(Id);
            }
            return new GroupModel(Id) { ParentId = ParentId, Name = Name };
        }

        /// <summary>
        /// Обновление группы
        /// </summary>
        /// <param name="model">Модель группы</param>
        /// <exception cref="ArgumentNullException">Ошибка при Null модели</exception>
        /// <exception cref="ArgumentException">Ошибка при неверном Id модели</exception>
        public void Update(GroupModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.Id == Guid.Empty || model.Id != Id)
            {
                throw new ArgumentException(nameof(model.Id));
            }
            SetFieldsByModel(model);
            _UpdateEvent?.Invoke(this,
                new UpdateEventArgs(Id, model.Fields.Clone() as FieldValueCollection));
        }

        /// <summary>
        /// Установить поля группы по модели
        /// </summary>
        /// <param name="model">Модель группы</param>
        /// <exception cref="ArgumentNullException">Ошибка, при Null значении</exception>
        private void SetFieldsByModel(GroupModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            foreach (var field in model.Fields)
            {
                switch (field.Key)
                {
                    case GroupScheme.ParentId:
                        ParentId = model.ParentId ?? Guid.Empty;
                        continue;
                    case GroupScheme.Name:
                        Name = model.Name;
                        continue;
                    default: continue;
                }
            }
        }
    }
}

/**
 * Неймспейс DataManagementServer
 * */
var dms = dms || {};

/**
 * Статичные значения
 * */
dms.static = Object.freeze({
    /**
     * Перечисления
     * */
    enums: Object.freeze({
        /*
         * Статус канала
         * */
        channelStatus: Object.freeze({
            /*
             * Хорошее качество
             * */
            good: 0,
            /*
             * Плохое качество
             * */
            bad: 1,
            /*
             * Нет соединения
             * */
            noConnection: 2
        }),

        /**
         * Типы плагинов
         * */
        pluginType: Object.freeze({
            /**
             * Стандартный
             * */
            defaultPlugin: 0,
            /**
             * Транспортировщик данных 
             * */
            dataTransporter: 1,
            /**
             * Обработчик данных
             * */
            dataProcessor: 2,
            /**
             * Отправитель оповещений
             * */
            notificationSender: 3
        }),

        /**
         * Статусы работы устройства
         * */
        deviceStatus: Object.freeze({
            /**
             * Нет статуса
             * */
            none: 0,
            /**
             * Создано
             * */
            created: 1,
            /**
             * Нормально работает
             * */
            runnig: 2,
            /**
             * Остановлено
             * */
            stoped: 3,
            /**
             * Ошибка
             * */
            error: 4
        }),

        /* 
         * Тип изменния коллекции
         * */
        collectionChangeType: Object.freeze({
            /**
             * Добавление элемента
             */
            addElement: 0,

            /**
             * Добавление элемента
             */
            deleteElement: 1
        })
    }),

    /*
     * Константы
     * */
    constants: Object.freeze({
        /*
         * Id корневой группы
         * */
        rootGroupId: "00000000-0000-0000-0000-000000000000"
    })
});

/**
 * Базовые методы для системы
 * */
dms.common = new function () {
    let pthis = this;

    /**
     * Показывает спиннер
     * @param {string} text Текст, отображаемый на спиннере
     */
    let showSpinner = function (text) {
        let w = 300;
        let h = 160;
        let spinnerDiv = window.top.document.createElement('div');
        spinnerDiv.setAttribute('id', "spinnerDiv");
        spinnerDiv.valign = "middle";
        spinnerDiv.align = "center";
        let divInnerHtml = "<table height='100%' width='100%' style='border: 3px solid black;cursor:wait'>";
        divInnerHtml += "<tr>";
        divInnerHtml += "<td vertical-align='middle' align='center'>";
        divInnerHtml += "<img id='loading' alt='' src='/img/dms_loading.gif'/>";
        divInnerHtml += "<div/><b>" + text + " ...</b>";
        divInnerHtml += "</td></tr></table>";
        spinnerDiv.innerHTML = divInnerHtml;
        spinnerDiv.style.background = '#FFFFFF';
        spinnerDiv.style.fontSize = "15px";
        spinnerDiv.style.zIndex = "1010";
        spinnerDiv.style.width = w + 'px';
        spinnerDiv.style.height = h + 'px';
        spinnerDiv.style.left = '50%';
        spinnerDiv.style.top = '50%';
        spinnerDiv.style.position = 'absolute';
        spinnerDiv.style.margin = '-80px 0 0 -150px';
        spinnerDiv.style.display = '';
        window.top.document.body.insertBefore(spinnerDiv, window.top.document.body.firstChild);
    };

    /**
     * Скрывает спиннер
     * */
    let hideSpinner = function () {
        let spinnerDiv = window.top.document.getElementById("spinnerDiv");
        if (spinnerDiv) {
            spinnerDiv.parentNode.removeChild(spinnerDiv);
        }
    };

    /**
     * Получает значение параметра из строки запроса
     * @param {string} params Набор параметров
     * @param {string} name Название нужного параметра
     */
    let getParamValue = function (params, name) {
        let result = "";
        if (!params || params == "") {
            return result;
        }

        let values = decodeURIComponent(params).split("&");
        for (let i in values) {
            values[i] = values[i].replace(/\+/g, " ").split("=");
            if (values[i][0] == name) {
                result = values[i][1];
                break;
            }
        }

        return result;
    };

    /**
     * Получает входные параметры формы
     * */
    let getDataParams = function () {
        let result = "";

        if (location.search == "") {
            return result;
        }

        let params = location.search.substr(1).split("&");
        for (let i in params) {
            params[i] = params[i].replace(/\+/g, " ").split("=");
        }

        for (let i in params) {
            if (params[i][0].toLowerCase() == "data") {
                result = params[i][1];
                break;
            }
        }

        return result;
    };

    pthis.showSpinner = showSpinner;
    pthis.hideSpinner = hideSpinner;
    pthis.getParamValue = getParamValue;
    pthis.getDataParams = getDataParams;
    return pthis;
}();
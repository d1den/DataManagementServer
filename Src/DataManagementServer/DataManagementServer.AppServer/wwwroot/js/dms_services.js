/**
 * Неймспейс DataManagementServer
 * */
var dms = dms || {};


/**
 * Сервис групп каналов
 * */
dms.groupService = new function () {
    let pthis = this;

    /**
     * Url сервиса групп
     * */
    let serviceUrl = "/api/Group";

    /**
     * Создать группу
     * */
    let createAsync = function () {
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/CreateGroup",
            processData: false
        });
    };

    /**
     * Создать группу в родительской группе
     * @param {string} parentGroupId Id родительской группы
     */
    let createInGroupAsync = function (parentGroupId) {
        let data = { parentId: parentGroupId };
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/CreateGroupInGroup?" + $.param(data, true),
            processData: false
        });
    }

    /**
     * Создать группу на основе модели
     * @param {object} modelData Модель группы
     * */
    let createByModelAsync = function (modelData) {
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/CreateGroupByModel",
            processData: false,
            data: JSON.stringify(modelData)
        });
    };

    /**
     * Обновить группу
     * @param {object} modelData Модель группы
     * */
    let updateAsync = function (modelData) {
        return $.ajax({
            type: "PATCH",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/UpdateGroupByModel",
            processData: false,
            data: JSON.stringify(modelData)
        });
    };

    /**
     * Получить группу
     * @param {string} groupId Id группы
     * */
    let getAsync = function (groupId) {
        if (!groupId) {
            return;
        }
        let data = { groupId: groupId };
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetGroup?" + $.param(data, true),
            processData: false
        });
    };

    /**
     * Получить группы по родительской
     * @param {string} parentGroupId Id родительской группы
     * @param {boolean} allFields Получить все поля?
     * @param {number} pageSize Размер страницы
     * @param {number} pageNumber Номер страницы
     * */
    let getByParentAsync = function (parentGroupId, allFields, pageSize, pageNumber) {
        let data = { parentId: parentGroupId, allFields: allFields };
        if (pageSize || pageNumber) {
            data["page.pageSize"] = pageSize;
            data["page.pageNumber"] = pageNumber;
        }

        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetGroupByParent?" + $.param(data, true),
            processData: false
        });
    };

    /**
     * Получить все группы
     * @param {boolean} allFields Получить все поля?
     * @param {number} pageSize Размер страницы
     * @param {number} pageNumber Номер страницы
     * */
    let getAllAsync = function (allFields, pageSize, pageNumber) {
        let data = { allFields: allFields };
        if (pageSize || pageNumber) {
            data["page.pageSize"] = pageSize;
            data["page.pageNumber"] = pageNumber;
        }

        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetAllGroup?" + $.param(data, true),
            processData: false
        });
    };


    /**
     * Удалить группу
     * @param {string} groupId Id группы
     * @param {boolean} withChildren С родительскими элементами?
     * */
    let deleteAsync = function (groupId, withChildren) {
        if (!groupId) {
            return;
        }

        let data = { groupId: groupId, withChildren: withChildren };
        return $.ajax({
            type: "DELETE",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/DeleteGroup?" + $.param(data, true),
            processData: false
        });
    };

    pthis.createAsync = createAsync;
    pthis.createInGroupAsync = createInGroupAsync;
    pthis.createByModelAsync = createByModelAsync;
    pthis.updateAsync = updateAsync;
    pthis.getAsync = getAsync;
    pthis.getByParentAsync = getByParentAsync;
    pthis.getAllAsync = getAllAsync;
    pthis.deleteAsync = deleteAsync;
    return pthis;
}();

/**
 * Сервис каналов
 * */
dms.channelService = new function () {
    let pthis = this;

    /**
     * Url сервиса каналов
     * */
    let serviceUrl = "/api/Channel";

    /**
     * Создать канал
     * */
    let createAsync = function () {
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/CreateChannel",
            processData: false
        });
    };

    /**
     * Создать канал в группе
     * @param {string} groupId Id родительской группы
     */
    let createInGroupAsync = function (groupId) {
        let data = { groupId: groupId };
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/CreateChannelInGroup?" + $.param(data, true),
            processData: false
        });
    }

    /**
     * Создать канал на основе модели
     * @param {object} modelData Модель канала
     * */
    let createByModelAsync = function (modelData) {
        return $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/CreateChannelByModel",
            processData: false,
            data: JSON.stringify(modelData)
        });
    };

    /**
     * Обновить канал
     * @param {object} modelData Модель канала
     * */
    let updateAsync = function (modelData) {
        return $.ajax({
            type: "PATCH",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/UpdateChannelByModel",
            processData: false,
            data: JSON.stringify(modelData)
        });
    };

    /**
     * Получить канал со всеми полями
     * @param {string} channelId Id канала
     * */
    let getAsync = function (channelId) {
        if (!channelId) {
            return;
        }
        let data = { channelId: channelId };
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetChannel?" + $.param(data, true),
            processData: false
        });
    };

    /**
     * Получить канал с выбранными полями
     * @param {string} channelId Id канала
     * @param {Array} fields Запрашиваемые поля 
     * */
    let getFieldsAsync = function (channelId, fields) {
        if (!channelId) {
            return;
        }
        let data = { channelId: channelId, fields: fields };
        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetChannelFields?" + $.param(data, true),
            processData: false
        });
    };

    /**
     * Получить каналы по родительской группе
     * @param {string} groupId Id родительской группы
     * @param {number} pageSize Размер страницы
     * @param {number} pageNumber Номер страницы
     * */
    let getByGroupAsync = function (groupId, pageSize, pageNumber) {
        let data = { groupId: groupId };
        if (pageSize || pageNumber) {
            data["page.pageSize"] = pageSize;
            data["page.pageNumber"] = pageNumber;
        }

        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetChannelByGroup?" + $.param(data, true),
            processData: false
        });
    };

    /**
     * Получить каналы по родительской группе с выбранными полями
     * @param {string} groupId Id родительской группы
     * @param {Array} fields Массив запрашиваемых полей
     * @param {number} pageSize Размер страницы
     * @param {number} pageNumber Номер страницы
     * */
    let getFieldsByGroupAsync = function (groupId, fields, pageSize, pageNumber) {
        let data = { groupId: groupId, fields: fields };
        if (pageSize || pageNumber) {
            data["page.pageSize"] = pageSize;
            data["page.pageNumber"] = pageNumber;
        }

        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetChannelByGroupFields?" + $.param(data, true),
            processData: false
        });
    };

    /**
     * Получить все каналы
     * @param {number} pageSize Размер страницы
     * @param {number} pageNumber Номер страницы
     * */
    let getAllAsync = function (pageSize, pageNumber) {
        let data = {};
        if (pageSize || pageNumber) {
            data["page.pageSize"] = pageSize;
            data["page.pageNumber"] = pageNumber;
        }

        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetChannelAll?" + $.param(data, true),
            processData: false
        });
    };


    /**
     * Получить все каналы с выбранными полями
     * @param {Array} fields Массив запрашиваемых полей
     * @param {number} pageSize Размер страницы
     * @param {number} pageNumber Номер страницы
     * */
    let getFieldsAllAsync = function (fields, pageSize, pageNumber) {
        let data = { fields: fields };
        if (pageSize || pageNumber) {
            data["page.pageSize"] = pageSize;
            data["page.pageNumber"] = pageNumber;
        }

        return $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/GetChannelAllFields?" + $.param(data, true),
            processData: false
        });
    };


    /**
     * Удалить канал
     * @param {string} channelId Id канала
     * */
    let deleteAsync = function (channelId) {
        if (!channelId) {
            return;
        }

        let data = { channelId: channelId };
        return $.ajax({
            type: "DELETE",
            contentType: "application/json; charset=utf-8",
            url: serviceUrl + "/DeleteChannel?" + $.param(data, true),
            processData: false
        });
    };

    pthis.createAsync = createAsync;
    pthis.createInGroupAsync = createInGroupAsync;
    pthis.createByModelAsync = createByModelAsync;
    pthis.updateAsync = updateAsync;
    pthis.getAsync = getAsync;
    pthis.getFieldsAsync = getFieldsAsync;
    pthis.getByGroupAsync = getByGroupAsync;
    pthis.getFieldsByGroupAsync = getFieldsByGroupAsync;
    pthis.getAllAsync = getAllAsync;
    pthis.getFieldsAllAsync = getFieldsAllAsync;
    pthis.deleteAsync = deleteAsync;
    return pthis;
}();
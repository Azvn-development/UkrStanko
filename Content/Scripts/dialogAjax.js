// Поиск элементов по полю в таблице
function searchElements(event, action) {
    if (event.keyCode === 13) {
        $.ajax({
            url: action,
            beforeSend: function () {
                $("#waiting-modal").fadeIn("fast");
            },
            complete: function () {
                $("#waiting-modal").fadeOut("fast");
            },
            data: { searchString: $(event.target).val(), startDate: $("#startDate").val(), endDate: $("#endDate").val(), ownRecords: $("#ownRecords").prop("checked") },
            success: function (result) {
                fillData(result);
            }
        })
    } // if
} // searchElements

// Поиск элементов по фильтру в диалоге
function filterElements (action) {
    $.ajax({
        url: action,
        data: { searchString: $("#Search").val(), startDate: $("#startDate").val(), endDate: $("#endDate").val(), ownRecords: $("#ownRecords").prop("checked") },
        success: function (result) {
            activeFilterButton();
            fillData(result)
        }
    })
} // filterElements

// Отправка сообщения
function sendMessage(action) {
    let messageText = $("#messageText").val();

    if (messageText.length > 0) {
        $.ajax({
            url: action,
            beforeSend: function () {
                $("#waiting-modal").fadeIn("fast");
            },
            complete: function () {
                $("#waiting-modal").fadeOut("fast");
            },
            data: { text: messageText, responseId: $("#responseId").val(), responseType: $("#responseType").val(), connectionId: sessionStorage.getItem("connectionId") },
            success: function (result) {
                fillData(result)
            }
        })
    } // if
} // sendMessage

// Удаление элемента из таблицы
function deleteElement(event, action) {
    let button = $(event.target);

    $.ajax({
        url: action,
        beforeSend: function () {
            $("#waiting-modal").fadeIn("fast");
        },
        complete: function () {
            $("#waiting-modal").fadeOut("fast");
        },
        data: {
            id: button.attr("id"),
            searchString: $("#Search").val(),
            startDate: $("#startDate").val(),
            endDate: $("#endDate").val(),
            ownRecords: $("#ownRecords").prop("checked"),
            connectionId: sessionStorage.getItem("connectionId"),
            details: button.attr("details")
        },
        success: function (result) {
            if (result == true) {
                $(button.attr("data-containerId")).remove();
                button.removeAttr("data-containerId");
            }
            else if (result != false) {
                if (button.attr("details") === 'true') fillData(result, "#main-data");
                else fillData(result);
            } // if
        }
    });
} // deleteElement

// Активация кнопки фильтра
function activeFilterButton() {
    let faFilter = $(".fa-filter");

    if ($("#startDate").val() == "" && $("#endDate").val() == "" && $("#ownRecords").prop("checked") == false) {
        faFilter.removeClass("open");
    }
    else {
        faFilter.addClass("open");
    } // if
} // activeFilterButton

// Заполнение данными, полученными из AJAX запроса
function fillData(result, targetId) {
    if (targetId != null) $(targetId).html(result);
    else $("#data").html(result, targetId);
} // fillData

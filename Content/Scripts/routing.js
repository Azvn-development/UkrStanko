// Возврат по истории назад
function goHistoryBack(currentUrl) {
    sessionStorage.setItem("historyBack", true);
    sessionStorage.setItem("currentUrl", currentUrl);

    sessionStorage.removeItem("previousUrl");

    window.history.back();
} // goHistoryBack

// AJAX переход по ссылке
function goToUrl(previousUrl, newUrl, reload) {
    // Проверка возврата
    if (sessionStorage.getItem("historyBack") == 'true') {
        // Проверка на возврат к форме
        if (checkForm(newUrl)) {
            window.history.back();
            return;
        } // if

        // Проверка на возврат по одинаковому URL
        if (checkSameUrl(newUrl)) {
            window.history.back();
            return;
        } // if
    } // if

    // Переход к новому url
    $.ajax({
        url: newUrl,
        data: { ajaxLoad: true, connectionId: sessionStorage.getItem("connectionId"), previousUrl: previousUrl },
        beforeSend: function () {
            $("#waiting-modal").fadeIn("fast");
        },
        complete: function () {
            $("#waiting-modal").fadeOut("fast");
        },
        success: function (result) {

            // Если страница не перезагружена и не осуществлен возврат назад,
            // то мы записываем ее в историю
            if (!reload) {
                window.history.pushState(null, null, newUrl);
            } // if

            // Очистка данных в локальном хранилище
            sessionStorage.removeItem("historyBack");
            sessionStorage.removeItem("currentUrl");

            $("#main-data").html(result);
        }
    });
} // goToUrl

// Проверка возврата к форме
function checkForm(newUrl) {
    return newUrl.includes("Create") || newUrl.includes("Edit");
} // checkForm

// Проверка возврата на одинаковый URL
function checkSameUrl(newUrl) {
    let currentUrl = sessionStorage.getItem("currentUrl");

    return currentUrl != null && currentUrl == newUrl;
} // checkSameUrl

// Изменение текущего URL в строке браузера при переходе с формы
function checkCurrentUrl(newUrl) {
    let currentUrl = window.location.href;

    if (checkForm(currentUrl)) window.history.pushState(null, null, newUrl);
} // checkCurrentUrl

// Запись текущего URL в хранилище
function saveUrl(currentUrl) {
    if (!currentUrl.includes("Create") && !currentUrl.includes("Edit")) {
        sessionStorage.setItem("previousUrl", currentUrl);
    } // if
} // saveUrl

// Запись сохраненного URL в поле и очистка хранилища
function getSavedUrl(field) {
    let previousUrl = sessionStorage.getItem("previousUrl");
    if (previousUrl != null) {
        field.val(previousUrl);
        sessionStorage.removeItem("previousUrl");
    } // if
} // getSavedUrl
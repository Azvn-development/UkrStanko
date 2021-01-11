// Скролл до первой непрочитанной новости при загрузке страницы
function scrollToFirstNewNotice() {
    let firstNewNotice = $(".first-new-notice");

    if (typeof firstNewNotice.offset() !== 'undefined') {
        let wt = $(window).scrollTop();
        let wh = $(window).height();
        let et = Math.ceil(firstNewNotice.offset().top);

        if (et >= wt + wh) {
            let scroll = et - wh - wt - 20;

            $('html, body').scrollTop(scroll);
        } // if
    } else {
        $('html, body').scrollTop($("main").outerHeight());
    } // if
} // scrollToFirstNewNotice

// Расчет отображения блока в области просмотра пользователя
function checkNoticeShow(action) {
    let firstNewNotice = $(".first-new-notice");

    if (typeof firstNewNotice.offset() !== 'undefined') {
        let wt = $(window).scrollTop();
        let wh = $(window).height();
        let et = Math.floor(firstNewNotice.offset().top);
        let eh = Math.floor(firstNewNotice.outerHeight());

        if (wt + wh >= et && wt + wh - eh * 2 <= et + (wh - eh)) {
            firstNewNotice.removeClass("new-notice");
            firstNewNotice.removeClass("first-new-notice");

            $(".info").children(".new-notice")
                .first()
                .addClass("first-new-notice");

            saveNewReadedNoticesCounter();
            sendNewReadedNoticesCounter(action);
        } // if
    } // if
} // checkNoticeShow

// Запись в хранилище начального значения счетчика прочитанных новостей
function saveStartReadedNoticesCounter(counter) {
    sessionStorage.setItem("startReadedNoticesCounter", counter);
    sessionStorage.removeItem("currentReadedNoticesCounter");
} // saveStartReadedNoticesCounter

// Инкрементирование в хранилище значения счетчика прочитанных новостей
function saveNewReadedNoticesCounter() {
    let currentCounter = sessionStorage.getItem("currentReadedNoticesCounter");

    if (currentCounter != null) {
        sessionStorage.setItem("currentReadedNoticesCounter", ++currentCounter);
    } else {
        currentCounter = sessionStorage.getItem("startReadedNoticesCounter");
        sessionStorage.setItem("currentReadedNoticesCounter", ++currentCounter);
    } // if
} // saveNewReadedNoticesCounter

// Запись в базу нового значения прочитанных новостей
function sendNewReadedNoticesCounter(action) {
    let startReadedNoticesCounter = sessionStorage.getItem("startReadedNoticesCounter");
    let currentReadedNoticesCounter = sessionStorage.getItem("currentReadedNoticesCounter");

    if (startReadedNoticesCounter != null &&
        currentReadedNoticesCounter != null &&
        startReadedNoticesCounter != currentReadedNoticesCounter) {
        $.ajax({
            url: action,
            data: { readedNoticeCouter: currentReadedNoticesCounter },
            success: function (result) {
                sessionStorage.setItem("startReadedNoticesCounter", result);
            }
        })
    } // if
} // sendNewReadedNoticesCounter
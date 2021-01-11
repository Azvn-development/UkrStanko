// Загрузка дополнительных записей
function downloadItems(action, containerId, downloadedCount, prepend) {
    let downloadItemsContainer = $("#downloadItemsContainer");

    $.ajax({
        url: action,
        data: { downloadedCount: downloadedCount },
        beforeSend: function () {
            $("#downloadItemsText").hide();
            $("#downloadItemsWheel").show();
        },
        complete: function () {
            $("#downloadItemsWheel").hide();
            $("#downloadItemsText").show();
        },
        success: function (result) {
            if (!prepend) $(containerId).append(result);
            else {
                $(containerId).prepend(result);
                scrollToDownloadItemsContainer(downloadItemsContainer);
            }

            downloadItemsContainer.remove();
        }
    })
} // downloadItems

// Скролл до первой непрочитанной новости при загрузке страницы
function scrollToDownloadItemsContainer(downloadItemsContainer) {
    if (typeof downloadItemsContainer.offset() !== 'undefined') {
        let wt = $(window).scrollTop();
        let wh = $(window).height();
        let et = Math.ceil(downloadItemsContainer.offset().top);

        if (et >= wt + wh) {
            let scroll = et - wh - wt;

            $('html, body').scrollTop(scroll);
        } // if
    } else {
        $('html, body').scrollTop($("main").outerHeight());
    } // if
} // scrollToDownloadItemsContainer
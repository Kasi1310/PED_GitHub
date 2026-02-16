function AjaxRequestJsonPost(url, jsonData, successcallback, errorCallback, isAsync) {
    ShowOverlay();
    if (isAsync === undefined) isAsync = true;
    $.ajax({
        cache: false,
        type: 'POST',
        async: isAsync,
        url: url,
        data: jsonData,
        success: function (resp, status, req) {
            SetSessionExpireTimer();
            HideOverlay();
            successcallback(resp, status, req);
        },
        error: function (req, status, error) {
            HideOverlay();
            CheckSessionTimeOut(req);
        },
        failure: function () {
            HideOverlay();
            alert("Error loading chart!");
        }
    });
}

function AjaxRequestJsonGet(url, jsonData, successcallback, errorCallback, isAsync) {
    ShowOverlay();
    
    if (isAsync === undefined) isAsync = true;
    $.ajax({
        cache: false,
        type: 'GET',
        async: isAsync,
        url: url,
        data: jsonData,
        success: function (resp, status, req) {
            SetSessionExpireTimer();
            HideOverlay();
            successcallback(resp, status, req);
        },
        error: function (req, status, error) {
            HideOverlay();
            CheckSessionTimeOut(req);
        },
        failure: function () {
            HideOverlay();
            alert("Error loading chart!");
        }
    });
}

function AjaxRequestJsonPostContent(url, jsonData, successcallback, errorCallback, isAsync) {
    ShowOverlay();    
    if (isAsync === undefined) isAsync = true;
    $.ajax({
        cache: false,
        type: 'POST',
        async: isAsync,
        url: url, 
        contentType: "application/json; charset=utf-8",
        data: jsonData,
        success: function (resp, status, req) {
            SetSessionExpireTimer();
            HideOverlay();
            successcallback(resp, status, req);
        },
        error: function (req, status, error) {
            HideOverlay();
            CheckSessionTimeOut(req);
        },
        failure: function () {
            HideOverlay();
            alert("Error loading chart!");
        }
    });
}

function CheckSessionTimeOut(req) {
    var url = '';
    if (req.status === 403) {
        var resp = $.parseJSON(req.responseText);

        if (resp.type === "SESSION_TIMEOUT") {
            url = $("#hrfUrlSessionExpiry").val();
            window.location.href = url;
        }
    }
    else {

        url = $("#hrfUrlAppError").val();
        window.location.href = url;
    }
}
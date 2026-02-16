function ApplicationError(req) {

    if (req.status === 403) {
        var resp = $.parseJSON(req.responseText);

        if (resp.type === "SESSION_TIMEOUT") {
            var url = $("#hrfUrlSessionExpiry").val();
            window.location.href = url;
        }
    }
    else {

        var url = $("#hrfUrlAppError").val();
        window.location.href = url;
    }
}

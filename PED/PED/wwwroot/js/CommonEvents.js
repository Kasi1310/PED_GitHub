function InitEvents() {
    //Cancel Chart
    $('#btnCancel').on('click', function () {
        swal({
            title: "Are you sure?",
            text: "Data in the page will be lost!",
            type: "info",
            showCancelButton: true,
            confirmButtonClass: 'btn-primary',
            confirmButtonText: 'Confirm'

        },
            function (isConfirm) {
                if (isConfirm) {
                    location.href = $('#hrfUrlList').val();
                }
            });
    });

    $('#lnktoggle').live('click', function () {
        $('#dvChartViewer').hide(1000, 'swing', function () {
            $('#divCodingContainer').toggleClass('col-md-6 col-md-12', 1000);
        });
        $('#toggleButton').toggleClass('hide show', 500);
    });

    $('#toggleButton').on('click', function () {
     
        //$("#divCodingContainer").toggleClass('col-md-12 col-md-6', 1000).promise().done(function () {
        $("#divCodingContainer").addClass('col-md-6', 1000).promise().done(function () {
            $('#dvChartViewer').show(1000, 'swing');
        });
        //$('#toggleButton').toggleClass('show hide', 500);
        $('#toggleButton').removeClass('show').addClass('hide', 500);
    })
}

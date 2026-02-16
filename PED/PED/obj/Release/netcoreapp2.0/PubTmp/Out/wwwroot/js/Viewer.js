function RenderViewer(sourcepage, sourcecontroller, paramfileid) {
    var fileid;
    
    if (paramfileid == null)
        fileid = $("#FileId").val();
    else
        fileid = paramfileid;
    $.ajax({
        //cache: false,
        type: "POST",
        url: $('#hrfUrlViewer').val(),
        data: { imageid: fileid, frompage: sourcepage, fromcontroller: sourcecontroller },
        success: function (resp) {
            SetSessionExpireTimer();
            $("#dvChartViewer").empty().html(resp);
            
        },
        error: function (req, status, error) {
            CheckSessionTimeOut(req);
        },
        failure: function () {
            alert("Error loading chart!");
        }
    });

    $(document).on('click', '[data-click="cac-panel-expand"]', function () {
        var targetContainer = '.cac-panel';
        var targetClass = 'active';

        if ($(targetContainer).hasClass(targetClass)) {
            $(targetContainer).attr('style', '');
            $(targetContainer).removeClass(targetClass);
        }
        else {
            $(targetContainer).addClass(targetClass);
        }

    });
}

function UncheckDiagCodes(ele) {
    if ($('#SliderPopup').val().toLowerCase() !== 'false') {
        if ($(ele).val() === '') {
            var id = $(ele).attr('id');
            var lastChar = id.slice(-1);

            $('#PageNo' + lastChar.toString()).val('');
            $('#DiagComments' + lastChar.toString()).val(null).trigger('change');
        }
    }
    ClearCodeMenuTree();
}

function ClearCodeMenuTree()
{
    $('#dvCodeMenuTree li').each(function (index, value) {
        var nodeid = $("#dvCodeMenuTree").jstree().get_node(this.id).id;
        var nodetext = $("#dvCodeMenuTree").jstree().get_node(this.id).text;
        var existFlag = false;
        $('.DiagList').each(function () {
            if ($.trim($(this).val()) == nodetext) {
                existFlag = true;
                return false;
            }
        });
        if (!existFlag) {
            $('#dvCodeMenuTree').jstree("uncheck_node", nodeid);
        }
    });
}

function LoadCodeTree(result) {
    var frompage = $("#FromPage").val().toLowerCase();
    $('#dvCodeMenuTree').jstree('destroy');
    $('#dvCodeMenuTree').jstree({
        'check_callback': true,
        'plugins': ["wholerow", "checkbox", "types", "state"],
        "core": {
            "themes": {
                "responsive": true,
                "icons": false
            },
            "data": result
        },
        checkbox: {
            three_state: false,
            whole_node: false,
            tie_selection: false
        },
    });


    $("#dvCodeMenuTree").on('ready.jstree', function (event, data) {
        $('#dvCodeMenuTree li').each(function (index, value) {
            var nodeid = $("#dvCodeMenuTree").jstree().get_node(this.id).id;
            var nodetext = $("#dvCodeMenuTree").jstree().get_node(this.id).text;
            $('.DiagList').each(function () {
                if ($.trim($(this).val()) == nodetext) {
                    data.instance.check_node(nodeid);
                    return false;
                }
            });
        });
    });

    $("#dvCodeMenuTree").on('refresh.jstree', function (event, data) {
        $('#dvCodeMenuTree li').each(function (index, value) {
            var nodeid = $("#dvCodeMenuTree").jstree().get_node(this.id).id;
            var nodetext = $("#dvCodeMenuTree").jstree().get_node(this.id).text;
            $('.DiagList').each(function () {
                if ($.trim($(this).val()) == nodetext) {
                    data.instance.check_node(nodeid);
                    return false;
                }
            });
        });
    });

    $('#dvCodeMenuTree').on("check_node.jstree", function (e, data) {
        var pgno = parseInt($("#PageIndexTextBox").val());
        var dataexists = false;
        var icd = data.node.text.toString();
        var diagRowLength = 0;
        var diagCount = 0;

        $('.DiagList').each(function () {
            diagRowLength = diagRowLength + 1;
            if ($.trim($(this).val()) == icd) {
                dataexists = true;
                return false;
            }
            if ($.trim($(this).val()) != '') {
                diagCount = diagCount + 1;
            }
        });

        if (diagRowLength == diagCount) {
            CloneRow('codereview', $('#divCodeEntryDetails'), diagRowLength);
            InitControls();
        }

        if (!dataexists) {
            var annotateid = data.node.id;

            UpdateAnnotationDetails(annotateid, 1);

            $('.DiagList').each(function () {
                if ($.trim($(this).val()) == '') {
                    var parentDiv = $(this).closest('.divRowCodeEntry');
                    $(parentDiv).find('.DiagList').val(icd);
                    $(parentDiv).find('.PageNo').val(pgno);
                    $(parentDiv).find('.DiagCommentList').select2("trigger", "select", {
                        data: { id: ['1'], text: '1-NewCode' }
                    });
                    return false;
                }
            });
        }
    });

    function UpdateAnnotationDetails(annotateid, actionid) {
        var url = $("#hrfUrlUpdAnnotation").val();
        var jsonData = { AnnotateId: annotateid, ActionId: actionid };

        $.ajax({
            cache: false,
            type: "POST",
            url: url,
            data: { AnnotateId: annotateid, ActionId: actionid },
            success: function (resp) {
                SetSessionExpireTimer();
                if (resp == "success") {
                    var pageno = parseInt($("#PageIndexTextBox").val());

                    var imageid = $("#FileId").val();

                    allRect = [];

                    allRectCoord = [];

                    DrawRetrieveRectangle(imageid, pageno);
                }
            },
            error: function (req, status, error) {
                CheckSessionTimeOut(req);
            },
            failure: function () {
                alert("Error in viewer!");
            }
        });
    }

    $('#dvCodeMenuTree').on("uncheck_node.jstree", function (e, data) {
        var pgno = parseInt($("#PageIndexTextBox").val());

        var nodetext = data.node.text;

        var annotateid = data.node.id;

        $('.DiagList').each(function () {
            if ($.trim($(this).val()) == nodetext) {
                var parentDiv = $(this).closest('.divRowCodeEntry');
                $(parentDiv).find('.DiagList').val('');
                $(parentDiv).find('.PageNo').val('');
                $(parentDiv).find('.DiagCommentList').val(null).trigger('change');
                return false;
            }
        });
        UpdateAnnotationDetails(annotateid, 3);
    });

    $("#dvCodeMenuTree").on("hover_node.jstree", function (e, data) {
        $(this).jstree().open_all(); // open all nodes so they are visible in dom

        $('#dvCodeMenuTree li').each(function (index, value) {
            var node = $("#dvCodeMenuTree").jstree().get_node(this.id);

            $("#" + node.id).tooltip();
        });

        $(this).jstree().close_all(); // close all again

        var txttip = data.node.original.a_attr

        $("#" + data.node.id).tooltip('close');

        $("#" + data.node.id).tooltip({
            items: $("#" + data.node.id),
            content: txttip,
            position: {
                my: "top-30",
                of: ".cac-panel-content",
                at: "top"
            },
        });

        $("#" + data.node.id).tooltip('open');
    });


}

function LoadCodeTreePopOut(result) {
    $('#dvCodeMenuTree').jstree('destroy');

    $('#dvCodeMenuTree').jstree({
        'check_callback': true,
        'plugins': ["wholerow", "checkbox", "types", "state"],
        "core": {
            "themes": {
                "responsive": true,
                "icons": false
            },
            "data": result
        },
        checkbox: {
            three_state: false,
            whole_node: false,
            tie_selection: false
        },
    });


    $("#dvCodeMenuTree").on('ready.jstree', function (event, data) {
        $('#dvCodeMenuTree li').each(function (index, value) {
            var nodeid = $("#dvCodeMenuTree").jstree().get_node(this.id).id;
            var nodetext = $("#dvCodeMenuTree").jstree().get_node(this.id).text;

            $('.DiagList').each(function () {
                if ($.trim($(this).val()) == nodetext) {
                    data.instance.check_node(nodeid);
                    return false;
                }
            });
        });
    });

    $("#dvCodeMenuTree").on('refresh.jstree', function (event, data) {
        $('#dvCodeMenuTree li').each(function (index, value) {
            var nodeid = $("#dvCodeMenuTree").jstree().get_node(this.id).id;
            var nodetext = $("#dvCodeMenuTree").jstree().get_node(this.id).text;

            $('.DiagList').each(function () {
                if ($.trim($(this).val()) == nodetext) {
                    data.instance.check_node(nodeid);
                    return false;
                }
            });
        });
    });

    $('#dvCodeMenuTree').on("check_node.jstree", function (e, data) {
        var pgno = parseInt($("#PageIndexTextBox").val());
        var dataexists = false;
        var icd = data.node.text.toString();
        var diagRowLength = 0;
        var diagCount = 0;

        $('.DiagList').each(function () {
            diagRowLength = diagRowLength + 1;
            if ($.trim($(this).val()) == icd) {
                dataexists = true;
                return false;
            }
            if ($.trim($(this).val()) != '') {
                diagCount = diagCount + 1;
            }
        });

        if (diagRowLength == diagCount) {
            CloneRow('codereview', $('#divCodeEntryDetails'), diagRowLength);
            InitControls();
        }

        if (!dataexists) {

            $('.DiagList').each(function () {
                if ($.trim($(this).val()) == '') {
                    var parentDiv = $(this).closest('.divRowCodeEntry');
                    $(parentDiv).find('.DiagList').val(icd);
                    $(parentDiv).find('.PageNo').val(pgno);
                    $(parentDiv).find('.DiagCommentList').select2("trigger", "select", {
                        data: { id: ['1'], text: '1-NewCode' }
                    });
                    return false;
                }
            });
        }
    });

    $('#dvCodeMenuTree').on("uncheck_node.jstree", function (e, data) {
        var pgno = parseInt($("#PageIndexTextBox").val());

        var nodetext = data.node.text;

        $('.DiagList').each(function () {
            if ($.trim($(this).val()) == nodetext) {
                var parentDiv = $(this).closest('.divRowCodeEntry');
                $(parentDiv).find('.DiagList').val('');
                $(parentDiv).find('.PageNo').val('');
                $(parentDiv).find('.DiagCommentList').val(null).trigger('change');
                return false;
            }
        });
    });

    $("#dvCodeMenuTree").on("hover_node.jstree", function (e, data) {
        $(this).jstree().open_all(); // open all nodes so they are visible in dom

        $('#dvCodeMenuTree li').each(function (index, value) {
            var node = $("#dvCodeMenuTree").jstree().get_node(this.id);

            $("#" + node.id).tooltip();
        });

        $(this).jstree().close_all(); // close all again

        var txttip = data.node.original.a_attr

        $("#" + data.node.id).tooltip('close');

        $("#" + data.node.id).tooltip({
            items: $("#" + data.node.id),
            content: txttip,
            position: {
                my: "left-5",
                at: "right-200"
            },
        });

        $("#" + data.node.id).tooltip('open');
    });
}

function fnClosePopup() {
    $("#divCodingContainer").toggleClass('col-md-12 col-md-6', 1000).promise().done(function () {
        $('#dvChartViewer').show(1000, 'swing', function () {
            $("#FromPage").val('');
            LoadPageImageUnload();
        });
    });
}
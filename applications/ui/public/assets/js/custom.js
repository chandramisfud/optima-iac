var toggleElement = null;
var toggle = KTToggle.getInstance(toggleElement);

var target = document.querySelector(".card_form");
var blockUI = new KTBlockUI(target, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

var heightContainer=0;
var is_create = 0;
var is_update = 0;

$(document).on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});

$('.form-select').on('select2:clear', (event) => {
    let idOnClear = event.target.id;
    let formSelect = $('.form-select');
    let arrFormSelect = Object.keys(formSelect).map((key) => formSelect[key]);
    if (arrFormSelect.length > 0) {
        arrFormSelect.forEach((item, index, arr) => {
            if (item.id) {
                if (item.id !== idOnClear) {
                    $('#'+item.id).select2('close');
                }
            }
        })
    }
});

$(document).ready(function () {

    $('.modal-header').on('mousedown', function () {
        $('.modal-dialog').draggable({ cancel: '.modal-title, .modal-body' });
    }).on('mouseover', function () {
        $('.modal-header').css('cursor', 'move');
        $('.modal-title').css('cursor', 'text');
    });

    Promise.all([getAccessCreate(), getAccessUpdate()]).then(async () => {
        if (is_create === 1 && is_update === 1) {
            $("#btn_download_template").removeClass('d-none');
            $("#btn-download-template").removeClass('d-none');
            $("#btn-upload").removeClass('d-none');
            $("#btn_upload").removeClass('d-none');
            $("#btn_send").removeClass('d-none');
        }
    });

    $(document).on( 'preInit.dt', function (e, settings) {
        $('.dataTables_scrollHeadInner tr').addClass('fw-bold fs-6 text-gray-800 bg-primary text-white');
        $('.dataTables_scrollHeadInner tr th').addClass('text-white');
        $('div.dataTables_scrollBody:not(.modal-body div.dataTables_scrollBody)').height( $(window).height() - heightContainer );
    });

    $(window).on('resize', function(){
        if ( $('.table').length > 0 ) {
            adaptDatatables(heightContainer);
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        }
    });

    $('.modal').on('shown.bs.modal', function (e) {
        if ( $('.table').length > 0 ) {
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        }
    });

    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        if ( $('.table').length > 0 ) {
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        }
    });

    //modal login profile
    $('#modal_popup_profile_change').on('shown.bs.modal', function () {
        $.get('/refresh-csrf').done(function(data) {
            $('meta[name="csrf-token"]').attr('content', data)
            $.ajaxSetup({
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                }
            });
            $.ajax({
                url         : '/auth/profile',
                type        : 'GET',
                async       : true,
                dataType    : 'JSON',
                cache       : false,
                contentType : false,
                processData : false,
                beforeSend: function() {

                },
                success : function(result) {
                    if(typeof result.islogin != 'undefined'){
                        window.location.href = '/login-page?islogin=1';
                    }else{
                        if (result.error) {

                        } else {
                            var data = [];
                            for (var j = 0; j < result.data.length; ++j) {
                                data.push({
                                    id: result.data[j].usergroupid,
                                    text: result.data[j].profileid,
                                    userlevel: result.data[j].userlevel,
                                });
                            }
                            sortResults(data, "text", "asc");
                            $('#profile').select2({
                                placeholder: "Select a profile",
                                dropdownParent: $('#modal_popup_profile_change'),
                                width: '100%',
                                data: data
                            });
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                },
                complete: function() {

                }
            });
        });
    });
});

$('#btn_back').on('click', function () {
    history.back();
});

const adaptDatatables = (heightContainer) => {
    $('div.dataTables_scrollBody').height( $(window).height() - heightContainer );
    $('div.dataTables_scrollBody:not(.modal-body div.dataTables_scrollBody)').height( $(window).height() - heightContainer );
}

const disableButtonSave = () => {
    $('#btn_save').attr('disabled', true);
    $('#btn_cancel').attr('disabled', true);
}

const enableButtonSave = () => {
    $('#btn_save').attr('disabled', false);
    $('#btn_cancel').attr('disabled', false);
}

const sortResults = (arr, prop, asc) => {
    arr.sort(function(a, b) {
        if (asc) {
            return (a[prop] > b[prop]) ? 1 : ((a[prop] < b[prop]) ? -1 : 0);
        } else {
            return (b[prop] > a[prop]) ? 1 : ((b[prop] < a[prop]) ? -1 : 0);
        }
    });
}

const checkFormAccess = (trxAction, id, url, popMsg, confirmButtonText = "Yes, delete it", type = "delete") => {
    $.ajax({
        url: "/check-form-access",
        type: "GET",
        data: {menuid: menu_id, access_name: trxAction},
        dataType: "JSON",
        async: true,
        success: function (result) {
            if (!result.error) {
                switch(trxAction) {
                    case "create_rec":
                        window.location.href = url;
                        break;
                    case "update_rec":
                        window.location.href = url;
                        break;
                    case "delete_rec":
                        Swal.fire({
                            title: swalTitle,
                            text: popMsg,
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#AAAAAA',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            cancelButtonText: 'No, cancel',
                            confirmButtonText: confirmButtonText,
                            reverseButtons: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                if(type == "activate"){
                                    fActivateRecord(id);
                                } else if (type == 'deactivate'){
                                    fDeactivateRecord(id);
                                } else {
                                    fDeleteRecord(id);
                                }
                            }
                        })
                        break;
                    default:
                    // code block
                }
            } else {
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status === 403) {
                Swal.fire({
                    title: swalTitle,
                    text: jqXHR.responseJSON.message,
                    icon: "warning",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: { confirmButton: "btn btn-optima" }
                }).then(function () {
                    window.location.href = '/login-page';
                });
            }
            console.log(jqXHR);
        },
    });
}

const assign = (target, source) => {
    var result = {}

    for (var i in target) result[i] = target[i]
    for (var i in source) result[i] = source[i]

    return result
}

const getAccessCreate = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/check-form-access",
            type: "GET",
            data: {menuid: menu_id, access_name: 'create_rec'},
            dataType: "JSON",
            success: function (result) {
                if(!result.error) {
                    if(!result.error) $("#btn_left_group").removeClass('d-none');
                    is_create = 1;
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getAccessUpdate = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/check-form-access",
            type: "GET",
            data: {menuid: menu_id, access_name: 'update_rec'},
            dataType: "JSON",
            success: function (result) {
                if(!result.error) {
                    is_update = 1;
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

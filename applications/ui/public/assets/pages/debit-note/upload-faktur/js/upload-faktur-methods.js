'use strict';

var dt_list_upload;
var swalTitle = "Upload Faktur";
heightContainer = 430;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_list_upload = $('#dt_list_upload').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        processing: true,
        serverSide: false,
        paging: true,
        ordering: false,
        searching: false,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Description',
                data: 'doc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Status',
                data: 'status',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });
});

$('#btn_download_template').on('click', function () {
    let url = file_host + '/assets/media/templates/Template_DN_FP.xlsx';
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_upload').on('click', function () {
    if ( document.getElementById("file").files.length > 0 ){
        dt_list_upload.clear().draw();
        let e = document.querySelector("#btn_upload");
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        blockUI.block();
        let formData = new FormData($('#form_upload_faktur')[0]);
        $.get('/refresh-csrf').done(function(data) {
            $('meta[name="csrf-token"]').attr('content', data);
            $.ajaxSetup({
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                }
            });
            $.ajax({
                url             : '/dn/upload-faktur/upload',
                data            : formData,
                type            : 'POST',
                async           : true,
                dataType        : 'JSON',
                cache           : false,
                contentType     : false,
                processData     : false,
                beforeSend: function () {
                },
                success: function (result, status, xhr, $form) {
                    if (!result.error) {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: { confirmButton: "btn btn-primary" }
                        }).then(function () {
                            let elListUploaded = $('#list_uploaded');
                            if (elListUploaded.hasClass('d-none')) elListUploaded.removeClass('d-none');
                            dt_list_upload.rows.add(result.data).draw();
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: { confirmButton: "btn btn-primary" }
                        });
                    }
                },
                complete: function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    blockUI.release();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-primary" }
                    });
                }
            });
        });
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please attach file",
            icon: "warning",
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

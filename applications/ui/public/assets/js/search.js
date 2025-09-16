'use strict';

let searchElement = document.querySelector("#kt_header_optima_search");
let searchOptima = new KTSearch(searchElement);

let searchWrapperElement = searchElement.querySelector('[data-kt-search-element="wrapper"]');
let searchFormElement = searchElement.querySelector('[data-kt-search-element="form"]');
let searchMainElement = searchElement.querySelector('[data-kt-search-element="main"]');
let searchResultsElement = searchElement.querySelector('[data-kt-search-element="results"]');
let searchEmptyElement = searchElement.querySelector('[data-kt-search-element="empty"]');

let resultSearch;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('form').submit(false);
})(window, document, jQuery);

searchOptima.on("kt.search.process", function() {
    let elResultSearch = $('#list_result_search');
    elResultSearch.html('');
    let keyword = searchOptima.getQuery();
    $('div[data-kt-search-element="spinner"]').addClass('d-none');
    $.ajax({
        url: "/search",
        type: "GET",
        data: {keyword:keyword},
        dataType: "JSON",
        success: function (res) {
            if (!res.error) {
                let result = res.data;
                if (result.length > 0) {
                    $('div[data-kt-search-element="results"]').removeClass('d-none');
                    $('div[data-kt-search-element="empty"]').addClass('d-none');
                    let html = "";
                    for (const item of result) {
                        let startYear = new Date(item.startPromo).getFullYear();
                        let url = '/';
                        if (item.tipe === "Promo") {
                            // let categoryId = await enc(item.categoryId);
                            let categoryShortDesc = item.categoryShortDesc;
                            url = '/promo/display/form?method=view&id=' + item.id + '&c=' + categoryShortDesc + '&sy=' + startYear + '&cycle=' + item.approvalCycle;
                        } else {
                            url = '/rpt/dn-display/form?&id=' + item.id;
                        }
                        html += '<a href="'+url+'" class="d-flex text-optima text-hover-darkblue align-items-center mb-2 list_result_search">\n' +
                            '                        <span class="fs-12px fw-bold">'+ item.refId +'</span>\n' +
                            '                    </a>';
                    }
                    elResultSearch.html(html);
                    searchOptima.complete();


                    $('.list_result_search').on('click', function () {
                        searchOptima.clear();
                        searchOptima.hide();
                    });
                } else {
                    $('div[data-kt-search-element="results"]').addClass('d-none');
                    $('div[data-kt-search-element="empty"]').removeClass('d-none');
                    searchOptima.complete();
                }
            } else {
                $('div[data-kt-search-element="results"]').addClass('d-none');
                $('div[data-kt-search-element="empty"]').removeClass('d-none');
                searchOptima.complete();
            }
        },
        complete:function(){
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
            $('div[data-kt-search-element="results"]').addClass('d-none');
            $('div[data-kt-search-element="empty"]').removeClass('d-none');
            searchOptima.complete();
        }
    });
});

searchOptima.on("kt.search.clear", function() {
    $('#list_result_search').html('');
    $('div[data-kt-search-element="results"]').addClass('d-none');
    $('div[data-kt-search-element="empty"]').addClass('d-none');
});

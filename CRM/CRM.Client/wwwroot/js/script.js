// global variables
let counter = 0;

function loadscripts() {

    $(function () {
        /////Open Full Menu On Hover Left Icon Menu/////
        // $('.menu-items-block').hover(function () {
        //     $('.menu-collapsed-mode').addClass('menu-sliding-mode');
        // },
        //     function () {
        //         $('.menu-collapsed-mode').removeClass('menu-sliding-mode');
        //     });


        // $('.menu-items-header-icon').click(function(){
        //     $('body').toggleClass('menu-open-mode');
        //     $('.menu-switcher').toggleClass('menu-collapsed-mode');
        // });

        $('.menu-items-header-icon').click(function () {
            if ($('#menu-not-open-mode').hasClass('menu-collapsed-mode')) {
                //do something it does have the protected class!
                $('#menu-not-open-mode').removeClass('menu-collapsed-mode');
                $('body').addClass('menu-open-mode');
            } else {
                $('#menu-not-open-mode').addClass('menu-collapsed-mode');
                $('body').removeClass('menu-open-mode');
            }
        });

        /////////////////////////////////////////////////////////////////////////

        /////Show Modal Function/////
        // $('.page-header-buttons-items.can-add').hover(function () {
        //     $(this).children('.page-header-plus').addClass('visible');

        // },
        //     function () {
        //         $(this).children('.page-header-plus').removeClass('visible');
        //     });

        /////////////////////////////////////////////////////////////////////////

        $('.menu-sliding-mode .menu-items-block .menu-items-header .menu-items-header-icon').click(function () {
            alert("test");
        });

        /////////////////////////////////////////////////////////////////////////

        /////Open Header Search Popup/////
        $('.setting-icon-header').click(function () {
            $('.setting-company-popup').addClass('d-block');
        });

        $('.close-popup-setting').click(function () {
            $('.setting-company-popup').removeClass('d-block');
        });

        /////////////////////////////////////////////////////////////////////////

        /////Open Main Search Popup/////
        $('.main-filter-search').click(function (e) {
            $('.search-popup-main').toggleClass('visible-flex');
            e.stopPropagation();
        });

        $(document).on("click", function (e) {
            if ($(e.target).is(".search-popup-main") === false) {


            }

            if (e.target.className !== "search-popup-main" && !$(e.target).parents('.search-popup-main').length)
                $(".search-popup-main").removeClass("visible-flex");
        });



        $(".save-company-setting-button").click(function () {
            $(".search-popup-main").removeClass("visible-flex");
        });

        $(".close-popup-setting").click(function () {
            $(".search-popup-main").removeClass("visible-flex");
        });
        
        let count = 0;
        count = (function increment() {
            if (counter < 3) {
                return counter += 1;
            }
        })();
        
        document.querySelector("body").addEventListener("click", e => {
            if (count !== undefined) {
                document.querySelector(".header-user-block")?.contains(e.target)
                    ? document.querySelector("#user-show-popup")?.classList.toggle("visible")
                    : document.querySelector("#user-show-popup")?.classList.remove("visible");
            }

            if (!e.target.className.includes("dropdown-toggle")) {
                $(".dropdown-menu.show").removeClass("show");
            }
        }, true);
    })
}


/////Change Bar style When Opened Modal/////
function OpenAddNew(){
    //$(".add-new-contact-button,  .add-new-contact-icon, .bar-notify-box").click(function () {
    $('.slide-panel').addClass('d-block');
    $('.right-bar').css('background-color', '#eef2f4');
    $('.help-icon-box').css('background-color', 'rgba(0,0,0,.1)');
    $('.help-icon').css('color', 'rgb(92, 97, 105)');;
    $('.notify-icon-box').css('background-color', 'rgba(0,0,0,.1)');
    $('.notify-icon').css('color', 'rgb(92, 97, 105)');
    $('.search-icon').css('color', 'rgb(92, 97, 105)');
    $('.bar-search-box').css('border-top', '1px solid rgba(140, 134, 134, 0.32)');
    //});
}

function CloseAddNew(){ 
    $('.slide-panel').removeClass('d-block');
    $('.right-bar').css('background-color', '')
    $('.help-icon-box').css('background-color', '')
    $('.help-icon').css('color', '')
    $('.notify-icon-box').css('background-color', '')
    $('.notify-icon').css('color', '')
    $('.search-icon').css('color', '')
    $('.bar-search-box').css('border-top', '')
}

window.dealScripts = {
    // toggle dropdown
    toggleDropdown(elm) {
        if (!$(elm).children(".dropdown-menu").hasClass("show")) {
            $(".dropdown-menu.show").removeClass("show");
            $(elm).children(".dropdown-menu").addClass("show");
        } else {
            $(elm).children(".dropdown-menu").removeClass("show");
        }
    }
}

function SelectDropDownText(id){
    document.getElementById(id).select();
}

window.dealStyles = function () {
    const labels = document.querySelectorAll(".readonly-input span > span");
    if (labels) {
        labels.forEach(label => {
            if (label.textContent) {
                label.style.height = `calc(100% - ${label.nextElementSibling.clientHeight}px)`;
            }
        });
    }
}

window.clipboardCopy = {
    copyText: function (textToBeCopied) {
        navigator.clipboard.writeText(textToBeCopied).then(function () {
            console.log("copied!");
            document.getElementById("clipboard-copied-icon").style.visibility ="visible";
            document.getElementById("clipboard-check-icon").style.visibility ="hidden";

            setTimeout(function () {
                document.getElementById("clipboard-copied-icon").style.visibility = "hidden";
                document.getElementById("clipboard-check-icon").style.visibility = "visible";
            }, 1000);
        })
            .catch(function (error) {
                console.log(error);
            });
    }
}

var langMap = {
    en: {
        path: 'English',
    },
    az: {
        path: 'Azerbaijan',
    },
    ru: {
        path: 'Russian',
    }
};
//function getLanguage() {
//    var lang = "ru";
//    var path = '//cdn.datatables.net/plug-ins/1.10.13/i18n/Azerbaijan.json';
//    var result = null;
//    $.ajax({
//        async: false,
//        url: path ,
//        success: function (obj) {
          
//        }
//    })
//    return result
//}
function ApplyjQueryDatatable() {
    $(document).ready(function () {
        $('#datatableId').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.12.1/i18n/az-AZ.json'
            },
            pageLength: 10,
            lengthMenu: [[10, 20, 50, -1], [10, 20, 50, 'All']]
        });
    });
}
$(document).ready(function () {

    /////Change Menu Style On Click Menu Icon/////
    $('.menu-items-header-icon').click(function(){
        $('.opened').toggleClass(' menu-open-mode');

        if ( $('#menu-not-open-mode').hasClass('menu-open-mode') ) {
            $('#menu-not-open-mode').removeClass('menu-sliding-mode');
            $('.logo').css('transition', '0.3s').css('left', '25px');
            $('.menu-switcher ').css('margin-left', '180px');
        }else{
            $('#menu-not-open-mode').addClass('menu-sliding-mode');
            $('.logo').css('transition','').css('left', '');
            $('.menu-switcher ').css('margin-left', '');
        }
    });
    /////////////////////////////////////////////////////////////////////////

    /////Show Popup On Click Profile Section/////
    $('.header-user-block').click(function (e) {
        $('#user-show-popup').toggleClass('visible');
        e.stopPropagation();

    });

    $(document.body).click(function () {

        $('#user-show-popup').removeClass('visible');
    });
    /////////////////////////////////////////////////////////////////////////


    $('.bar-search-box').click(function () {
        $('.search-slide-panel').addClass('d-block-animation');
    });

    $('.slide-panel-label').click(function() {
        $('.search-slide-panel').removeClass('d-block-animation');
    })

    /////Change Bar style When Opened Modal/////
    $(".add-new-contact-button,  .add-new-contact-icon, .bar-notify-box").click(function () {
        $('.slide-panel').addClass('d-block-animation');
        $('.right-bar').css('background-color', '#eef2f4')
        $('.help-icon-box').css('background-color', 'rgba(0,0,0,.1)')
        $('.help-icon').css('color', 'rgb(92, 97, 105)')
        $('.notify-icon-box').css('background-color', 'rgba(0,0,0,.1)')
        $('.notify-icon').css('color', 'rgb(92, 97, 105)')
        $('.search-icon').css('color', 'rgb(92, 97, 105)')
        $('.bar-search-box').css('border-top', '1px solid rgba(140, 134, 134, 0.32)')
    });

    $('.slide-panel-label').click(function () {
        $('.slide-panel').removeClass('d-block-animation');
        $('.right-bar').css('background-color', '')
        $('.help-icon-box').css('background-color', '')
        $('.help-icon').css('color', '')
        $('.notify-icon-box').css('background-color', '')
        $('.notify-icon').css('color', '')
        $('.search-icon').css('color', '')
        $('.bar-search-box').css('border-top', '')
    });
    /////////////////////////////////////////////////////////////////////////

    var delay = 1000, MyTimeOut;//the delay interval
    $(".menu-items-block").hover(function () {
        if(!$('#menu-not-open-mode').hasClass('menu-open-mode')){
            MyTimeOut= setTimeout(function () {
                
                $('.opened').addClass('menu-sliding-mode');
            }, delay);
        }
    },
    function () {
        clearTimeout(MyTimeOut);
        $('.opened').removeClass('menu-sliding-mode');
    });

    /////Change Background White On Click Header Search Input/////
    $('.search-form').click(function () {
        $('.header-search-inner').addClass('header-search-active');
    });

    $('.search-form').focusout(function InputBlur() {
        $('.header-search-inner').removeClass('header-search-active');
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
    $('.main-filter-search').click(function () {
        $('.search-popup-main').addClass('visible-flex');
    });

    $('.close-popup-setting').click(function () {
        $('.search-popup-main').removeClass('visible-flex');
    });
    /////////////////////////////////////////////////////////////////////////

    /////Open Call Dial/////
    $('.bar-dial-box').click(function(){
        $('.phone-call-popup-keypad').toggleClass('visible');
    });
    /////////////////////////////////////////////////////////////////////////

    /////Delete Number/////
    $('#deletenumber').on('click',function () { 
        //get the input's value
        var textVal = $('#keypad-body-panel-input').val();
        //set the input's value
        $('#keypad-body-panel-input').val(textVal.substring(0,textVal.length - 1));
    });
    /////////////////////////////////////////////////////////////////////////

    /////Number Format For Call/////
    var inputEl = document.getElementById('keypad-body-panel-input');
    var goodKey = '0123456789+#* ';

    var checkInputTel = function(e) {
    var key = (typeof e.which == "number") ? e.which : e.keyCode;
    var start = this.selectionStart,
    end = this.selectionEnd;

    var filtered = this.value.split('').filter(filterInput);
    this.value = filtered.join("");

    /* Prevents moving the pointer for a bad character */
    var move = (filterInput(String.fromCharCode(key)) || (key == 0 || key == 8)) ? 0 : 1;
    this.setSelectionRange(start - move, end - move);
    }

    var filterInput = function(val) {
    return (goodKey.indexOf(val) > -1);
    }

    inputEl.addEventListener('input', checkInputTel);
    /////////////////////////////////////////////////////////////////////////


    /////Click Number/////
    $(".keypad-body-button").click(function () {
        var input = $("#keypad-body-panel-input");
        var attrmethod = $(this).children().attr('data-number');
        input.val( input.val() + attrmethod );
    });
    /////////////////////////////////////////////////////////////////////////


    // $(".keypad-body-button-0").on({
    //     mousedown: function() {
    //         $(this).data('timer', setTimeout(function() {
    //             var input = $("#keypad-body-panel-input");
    //             input.val( input.val() + "+" );
    //         }, 1000));
    //     },
    //     mouseup: function() {
    //         clearTimeout( $(this).data('timer') );
    //         alert("test");
    //     }
    // });
});
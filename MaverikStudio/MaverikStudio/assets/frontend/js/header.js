let body = document.querySelector('body');
let menuIconWrapper = document.querySelector('.header__middle-container-left-wrapper');
let menuIcon = document.querySelector('.header__middle-container-left-menu');
let triangleMenuMobile = document.querySelector('.header__middle-container-left-wrapper>.box-triangle');
let formMenu = document.querySelector('.header__middle-container-left-menu-mobile');

menuIconWrapper.addEventListener('click', (e) => {
    menuIcon.classList.toggle('open-menu');
    if (iconUser.className == "fa-light fa-xmark") {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
        formUser.classList.remove('show');
        triangleUser.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (iconSearch.className == "fa-light fa-xmark") {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px";
        formSearch.classList.remove('show');
        triangleSearch.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (iconCart.className == "fa-light fa-xmark") {
        iconCart.className = "fa-light fa-cart-shopping";
        iconCart.style = "font-size: 20px; margin-left: 8px; padding: 4px;";
        formCart.classList.remove('show');
        triangleCart.classList.remove('show');
        body.classList.remove('locked-scroll');
    }

    formMenu.classList.toggle('show');
    triangleMenuMobile.classList.toggle('show');
    body.classList.toggle('locked-scroll');
})

formMenu.addEventListener('click', (e) => {
    e.stopPropagation();
})

triangleMenuMobile.addEventListener('click', (e) => {
    e.stopPropagation();
})

//event focus input search
let inputSearch = document.querySelector('.header__middle-search-box-input');
let boxResultSearch = document.querySelector('#resultBigSearch');
let inputMobileSearch = document.querySelector('#mobileSearch');
let boxResultMobileSearch = document.querySelector('#resultMobileSearch');

inputSearch.addEventListener('focus', (e) => {
    setTimeout(function () {
        boxResultSearch.style = 'display: block;';
    }, 100)
});

inputSearch.addEventListener('blur', (e) => {
    setTimeout(function () {
        boxResultSearch.style = 'display: none;';
    }, 100)
});

inputMobileSearch.addEventListener('focus', (e) => {
    setTimeout(function () {
        boxResultMobileSearch.style = 'display: block;';
    }, 100)
});

inputMobileSearch.addEventListener('blur', (e) => {
    setTimeout(function () {
        boxResultMobileSearch.style = 'display: none;';
    }, 100)
});

//Handle show search, user, cart header right
let iconSearch = document.querySelector('#searchHeaderRight');
let triangleSearch = document.querySelector('#searchHeaderRight~.box-triangle');
let formSearch = document.querySelector('.header__middle-container-right-form-search');
let iconUser = document.querySelector('#userHeaderRight');
let triangleUser = document.querySelector('#userHeaderRight~.box-triangle');
let formUser = document.querySelector('.header__middle-container-right-form-user');
let iconCart = document.querySelector('#cartHeaderRight');
let triangleCart = document.querySelector('#cartHeaderRight~.box-triangle');
let formCart = document.querySelector('.header__middle-container-right-form-cart');
let container = document.querySelector('.container');
let footer = document.querySelector('.footer');

container.addEventListener('click', () => {
    if (iconUser.className == "fa-light fa-xmark") {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
        formUser.classList.remove('show');
        triangleUser.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (iconSearch.className == "fa-light fa-xmark") {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px";
        formSearch.classList.remove('show');
        triangleSearch.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (iconCart.className == "fa-light fa-xmark") {
        iconCart.className = "fa-light fa-cart-shopping";
        iconCart.style = "font-size: 20px; margin-left: 8px; padding: 4px;";
        formCart.classList.remove('show');
        triangleCart.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (menuIcon.classList.contains('open-menu')) {
        menuIcon.classList.remove('open-menu');
        formMenu.classList.remove('show');
        triangleMenuMobile.classList.remove('show');
        body.classList.remove('locked-scroll');
    }
})

iconSearch.addEventListener('click', () => {
    if (iconUser.className == "fa-light fa-xmark") {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
        formUser.classList.remove('show');
        triangleUser.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (iconCart.className == "fa-light fa-xmark") {
        iconCart.className = "fa-light fa-cart-shopping";
        iconCart.style = "font-size: 20px; margin-left: 8px; padding: 4px;";
        formCart.classList.remove('show');
        triangleCart.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (menuIcon.classList.contains('open-menu')) {
        menuIcon.classList.remove('open-menu');
        formMenu.classList.remove('show');
        triangleMenuMobile.classList.remove('show');
        body.classList.remove('locked-scroll');
    }

    if (iconSearch.className == "fa-light fa-magnifying-glass") {
        iconSearch.className = "fa-light fa-xmark";
        iconSearch.style = "font-size: 28px;";
    }
    else {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px;";
    }
    formSearch.classList.toggle('show');
    triangleSearch.classList.toggle('show');
    body.classList.toggle('locked-scroll');
})

iconUser.addEventListener('click', () => {
    if (iconSearch.className == "fa-light fa-xmark") {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px";
        formSearch.classList.remove('show');
        triangleSearch.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (iconCart.className == "fa-light fa-xmark") {
        iconCart.className = "fa-light fa-cart-shopping";
        iconCart.style = "font-size: 20px; margin-left: 8px; padding: 4px;";
        formCart.classList.remove('show');
        triangleCart.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (menuIcon.classList.contains('open-menu')) {
        menuIcon.classList.remove('open-menu');
        formMenu.classList.remove('show');
        triangleMenuMobile.classList.remove('show');
        body.classList.remove('locked-scroll');
    }

    if (iconUser.className == "fa-light fa-user") {
        iconUser.className = "fa-light fa-xmark";
        iconUser.style = "font-size: 28px; padding: 0px 2.25px 0px";
    } else {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
    }
    formUser.classList.toggle('show');
    triangleUser.classList.toggle('show');
    body.classList.toggle('locked-scroll');
})

iconCart.addEventListener('click', () => {
    if (iconUser.className == "fa-light fa-xmark") {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
        formUser.classList.remove('show');
        triangleUser.classList.remove('show');
        body.classList.remove('locked-scroll');
    }
    else if (iconSearch.className == "fa-light fa-xmark") {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px;";
        formSearch.classList.remove('show');
        triangleSearch.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if (menuIcon.classList.contains('open-menu')) {
        menuIcon.classList.remove('open-menu');
        formMenu.classList.remove('show');
        triangleMenuMobile.classList.remove('show');
        body.classList.remove('locked-scroll');
    }

    if (iconCart.className == "fa-light fa-cart-shopping") {
        iconCart.className = "fa-light fa-xmark";
        iconCart.style = "font-size: 28px; margin-left: 9.5px; padding: 0px 4px;";
    } else {
        iconCart.className = "fa-light fa-cart-shopping";
        iconCart.style = "font-size: 20px; margin-left: 8px; padding: 4px;";
    }
    formCart.classList.toggle('show');
    triangleCart.classList.toggle('show');
    body.classList.toggle('locked-scroll');
})


//Handle menu scroll
let menu = document.querySelector('#idHeader');
let tempScroll = 0;
let idBody = document.querySelector('#idBody');

window.addEventListener('scroll', () => {
    if (!idBody.classList.contains('locked-scroll')) {
        if (window.scrollY > 333) {
            if (!menu.classList.contains("menu-start")) {
                menu.className = "header menu-start";
            } else if (window.scrollY > 500 && !menu.classList.contains("menu-down")) {
                menu.className = "header menu-start menu-down";
                container.click();
            } else if (window.scrollY > 500 && window.scrollY < tempScroll) {
                if (!menu.classList.contains("menu-up")) {
                    menu.className = "header menu-start menu-down menu-up";
                    container.click();
                }
            } else if (window.scrollY > tempScroll) {
                if (menu.classList.contains("menu-up")) {
                    menu.className = "header menu-start menu-down";
                    container.click();
                }
            } else if (!menu.classList.contains("menu-up")) {
                menu.className = "header menu-start";
            }
        }
        else if (window.scrollY < 300) {
            menu.className = "header";
        }
        tempScroll = window.scrollY;
    }
})
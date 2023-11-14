let body = document.querySelector('body');
let menuIconWrapper = document.querySelector('.header__middle-container-left-wrapper');
let menuIcon = document.querySelector('.header__middle-container-left-menu');
let triangleMenuMobile = document.querySelector('.header__middle-container-left-wrapper>.box-triangle');
let formMenu = document.querySelector('.header__middle-container-left-menu-mobile');

menuIconWrapper.addEventListener('click', (e) => {
    menuIcon.classList.toggle('open-menu');
    if(iconUser.className == "fa-light fa-xmark") {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
        formUser.classList.remove('show');
        triangleUser.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(iconSearch.className == "fa-light fa-xmark") {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px";
        formSearch.classList.remove('show');
        triangleSearch.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(iconCart.className == "fa-light fa-xmark") {
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
let boxResultSearch = document.querySelector('.header__middle-search-box-result');

inputSearch.addEventListener('focus', (e) => {
    boxResultSearch.style = 'display: block;';
});

inputSearch.addEventListener('blur', (e) => {
    boxResultSearch.style = 'display: none;';
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
    if(iconUser.className == "fa-light fa-xmark") {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
        formUser.classList.remove('show');
        triangleUser.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(iconSearch.className == "fa-light fa-xmark") {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px";
        formSearch.classList.remove('show');
        triangleSearch.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(iconCart.className == "fa-light fa-xmark") {
        iconCart.className = "fa-light fa-cart-shopping";
        iconCart.style = "font-size: 20px; margin-left: 8px; padding: 4px;";
        formCart.classList.remove('show');
        triangleCart.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(menuIcon.classList.contains('open-menu')) {
        menuIcon.classList.remove('open-menu');
        formMenu.classList.remove('show');
        triangleMenuMobile.classList.remove('show');
        body.classList.remove('locked-scroll');
    }
})

iconSearch.addEventListener('click', () => {
    if(iconUser.className == "fa-light fa-xmark") {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
        formUser.classList.remove('show');
        triangleUser.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(iconCart.className == "fa-light fa-xmark") {
        iconCart.className = "fa-light fa-cart-shopping";
        iconCart.style = "font-size: 20px; margin-left: 8px; padding: 4px;";
        formCart.classList.remove('show');
        triangleCart.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(menuIcon.classList.contains('open-menu')) {
        menuIcon.classList.remove('open-menu');
        formMenu.classList.remove('show');
        triangleMenuMobile.classList.remove('show');
        body.classList.remove('locked-scroll');
    }

    if(iconSearch.className == "fa-light fa-magnifying-glass") {
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
    if(iconSearch.className == "fa-light fa-xmark") {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px";
        formSearch.classList.remove('show');
        triangleSearch.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(iconCart.className == "fa-light fa-xmark") {
        iconCart.className = "fa-light fa-cart-shopping";
        iconCart.style = "font-size: 20px; margin-left: 8px; padding: 4px;";
        formCart.classList.remove('show');
        triangleCart.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(menuIcon.classList.contains('open-menu')) {
        menuIcon.classList.remove('open-menu');
        formMenu.classList.remove('show');
        triangleMenuMobile.classList.remove('show');
        body.classList.remove('locked-scroll');
    }

    if(iconUser.className == "fa-light fa-user") {
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
    if(iconUser.className == "fa-light fa-xmark") {
        iconUser.className = "fa-light fa-user";
        iconUser.style = "font-size: 20px; padding: 4px;";
        formUser.classList.remove('show');
        triangleUser.classList.remove('show');
        body.classList.remove('locked-scroll');
    }
    else if(iconSearch.className == "fa-light fa-xmark") {
        iconSearch.className = "fa-light fa-magnifying-glass";
        iconSearch.style = "font-size: 20px;";
        formSearch.classList.remove('show');
        triangleSearch.classList.remove('show');
        body.classList.remove('locked-scroll');
    } else if(menuIcon.classList.contains('open-menu')) {
        menuIcon.classList.remove('open-menu');
        formMenu.classList.remove('show');
        triangleMenuMobile.classList.remove('show');
        body.classList.remove('locked-scroll');
    }

    if(iconCart.className == "fa-light fa-cart-shopping") {
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
    if(!idBody.classList.contains('locked-scroll')) {
        if(window.scrollY > 333) {
            if(!menu.classList.contains("menu-start")) {
                menu.className = "header menu-start";
            } else if(window.scrollY > 500 && !menu.classList.contains("menu-down")) {
                menu.className = "header menu-start menu-down";
                container.click();
            } else if(window.scrollY > 500 && window.scrollY < tempScroll) {
                if(!menu.classList.contains("menu-up")) {
                    menu.className = "header menu-start menu-down menu-up";
                    container.click();
                }
            } else if(window.scrollY > tempScroll) {
                if(menu.classList.contains("menu-up")) {
                    menu.className = "header menu-start menu-down";
                    container.click();
                }
            } else if(!menu.classList.contains("menu-up")) {
                menu.className = "header menu-start";
            }
        }
        else if(window.scrollY < 300) {
            menu.className = "header";
        }
        tempScroll = window.scrollY;
    }
})

//Handle menu mobile
let iconMenuMobile = document.querySelectorAll('.menu-sidebar-icon');
let barTitleMenuMobile = document.querySelector('.container__sidebar-menu .container__sidebar-menu-title');
let iconBarTitleMenuMobile = document.querySelector('.container__sidebar-menu .container__sidebar-menu-title span i');
let barMenuMobile = document.querySelector('.container__sidebar-menu .container__sidebar-menu-content');
let heightContentMenuMobile;
let barTitleBrandsMobile = document.querySelector('.container__sidebar-brands .container__sidebar-menu-title');
let iconBarTitleBrandsMobile = document.querySelector('.container__sidebar-brands .container__sidebar-menu-title span i');
let barBrandsMobile = document.querySelector('.container__sidebar-brands .container__sidebar-brands-content');
let heightContentBrandsMobile;
let enableMenuSidebarClick = true;

if(barMenuMobile.offsetHeight == 0) {
    barMenuMobile.style.display = 'block';
    barMenuMobile.style.height = 'auto';
    heightContentMenuMobile = barMenuMobile.offsetHeight;
    barMenuMobile.style.display = 'none';
    barMenuMobile.style.height = '0px';
} else {
    heightContentMenuMobile = barMenuMobile.offsetHeight;
}

if(barBrandsMobile.offsetHeight == 0) {
    barBrandsMobile.style.display = 'block';
    barBrandsMobile.style.height = 'auto';
    heightContentBrandsMobile = barBrandsMobile.offsetHeight;
    barBrandsMobile.style.display = 'none';
    barBrandsMobile.style.height = '0px';
} else {
    heightContentBrandsMobile = barBrandsMobile.offsetHeight;
}



for(let i=0;i<iconMenuMobile.length;i++) {
    iconMenuMobile[i].addEventListener('click', () => {
        let subMenuMobile = iconMenuMobile[i].closest('li').querySelector('.sub-menu-sidebar');
        if(subMenuMobile) {
            subMenuMobile.classList.toggle('show');
        }
        barMenuMobile.style.height = 'auto';
        heightContentMenuMobile = barMenuMobile.offsetHeight;
    }) 
}


function HandleHeight(element, intervalOrg, durationOrg, heightOrg) {
    let box = document.querySelector(element); // Thay 'tenBox' bằng id của box bạn muốn giảm chiều cao
    let height = heightOrg;

    let interval = intervalOrg; // Độ chia nhỏ chiều cao
    let duration = durationOrg; // Thời gian giảm chiều cao (miligiây)
    let steps = duration / interval;
    let perStep = height / steps;

    let check;
    if(box.style.display === 'block') {
        check = true;
    } else {
        check = false;
        box.style.height = '0px';
        box.style.display = 'block';
    }

    box.style.overflow = 'hidden';
    let i = 0;
    isIntervalMenuMobile = setInterval(function() {
        if (i < steps) {
            if (check) {
                box.style.height = (height - i * perStep) + 'px';
            } else {
                box.style.height = (i * perStep) + 'px';
            }
            i++;
        } else {
            clearInterval(isIntervalMenuMobile);
            if(check) {
                box.style.height = '0px'; // Đảm bảo chiều cao cuối cùng là 0
                box.style = 'display: none;';
            } else {
                box.style.height = 'auto';
                box.style = 'display: block;';
            }
            enableMenuSidebarClick = true;
        }
    }, interval);
}

barTitleMenuMobile.addEventListener('click', () => {
    if(enableMenuSidebarClick) {
        if(barMenuMobile.offsetHeight == 0) {
            enableMenuSidebarClick = false;
            iconBarTitleMenuMobile.classList.toggle('rotate180');
            HandleHeight('.container__sidebar-menu-content', 16.67, 300, heightContentMenuMobile);
        } else if(barMenuMobile.offsetHeight == heightContentMenuMobile) {
            enableMenuSidebarClick = false;
            iconBarTitleMenuMobile.classList.toggle('rotate180');
            HandleHeight('.container__sidebar-menu-content', 16.67, 300, heightContentMenuMobile);
        }
    }
})

barTitleBrandsMobile.addEventListener('click', () => {
    if(enableMenuSidebarClick) {
        if(barBrandsMobile.offsetHeight == 0) {
            enableMenuSidebarClick = false;
            iconBarTitleBrandsMobile.classList.toggle('rotate180');
            HandleHeight('.container__sidebar-brands-content', 16.67, 300, heightContentBrandsMobile);
        } else if(barBrandsMobile.offsetHeight == heightContentBrandsMobile) {
            enableMenuSidebarClick = false;
            iconBarTitleBrandsMobile.classList.toggle('rotate180');
            HandleHeight('.container__sidebar-brands-content', 16.67, 300, heightContentBrandsMobile);
        }
    }
})

//Handle filter checkbox
let labelFilter = document.querySelectorAll('.container__sidebar-brands-content label');

for(let i = 0; i<labelFilter.length; i++) {
    labelFilter[i].addEventListener('click', () => {
        let liFilter = labelFilter[i].closest('li');
        liFilter.classList.toggle('checked');
        let checkBox = liFilter.querySelector('input[type="checkbox"]');
        checkBox.checked = !checkBox.checked;
    })
}

//Handle slider
let controlLeft = document.querySelector('.slider-control-left');
let controlRight = document.querySelector('.slider-control-right');
let sliderListItem = document.querySelector('.slider-list-item');
let sliderItems = sliderListItem.querySelectorAll('.slider-item');
let enableClickLeft = true;
let enableClickRight = true;

let firstSliderItem = sliderListItem.querySelector('.slider-item:first-child');

sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0.25s ease 0s;`;

window.addEventListener('resize', () => {
    firstSliderItem = sliderListItem.querySelector('.slider-item:first-child');
    sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0.25s ease 0s;`;
})

sliderListItem.addEventListener("transitionend", function(event) {
    if (event.propertyName === "transform") {
        let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

        let translateXValue = match ? parseFloat(match[1]) : 0;

        if(translateXValue <= -(firstSliderItem.offsetWidth*11)) {
            sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3 - 1)}px); transition: all 0s ease 0s;`;
        }

        if(translateXValue >= 0) {
            sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0s ease 0s;`;
        }

        enableClickRight = true;
        enableClickLeft = true;
    }
});

//Sang trái dương
controlLeft.addEventListener('click', (e) => {
    if(enableClickLeft) {
        let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

        let translateXValue = match ? parseFloat(match[1]) : 0;

        sliderListItem.style = `transform: translateX(${translateXValue + firstSliderItem.offsetWidth}px); transition: all 0.25s ease 0s;`;

        enableClickLeft = false;
    }
})

controlLeft.addEventListener('mousedown', (e) => {
    e.stopPropagation();
})

//Sang phải âm
controlRight.addEventListener('click', (e) => {
    if(enableClickRight) {
        // Sử dụng regex để trích xuất giá trị translateX
        let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

        // Nếu có kết quả, lấy giá trị từ nhóm nằm trong dấu ngoặc
        let translateXValue = match ? parseFloat(match[1]) : 0;

        sliderListItem.style = `transform: translateX(${translateXValue - firstSliderItem.offsetWidth}px); transition: all 0.25s ease 0s;`;

        enableClickRight = false;
    }
})

controlRight.addEventListener('mousedown', (e) => {
    e.stopPropagation();
    console.log('a');
})

//Handle grabbing pull slider
let isDragging;
let temp2;
let offsetX;

sliderListItem.addEventListener("mousedown", function(e) {
    isDragging = true;
    offsetX = e.clientX - sliderListItem.getBoundingClientRect().left;
    sliderListItem.classList.add('pull');
});

document.addEventListener("mousemove", function(e) {
    if (isDragging) {
        let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

        let translateXValue = match ? parseFloat(match[1]) : 0;

        if(translateXValue <= -(firstSliderItem.offsetWidth*11)) {
            sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3 - 1)}px); transition: all 0s ease 0s;`;
            offsetX = e.clientX - sliderListItem.getBoundingClientRect().left;
            return;
        } else if(translateXValue >= 0) {
            sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0s ease 0s;`;
            offsetX = e.clientX - sliderListItem.getBoundingClientRect().left;
            return;
        }

        let x = e.clientX - offsetX;
        sliderListItem.style.transform = "translateX(" + x + "px)";
    }
});

document.addEventListener("mouseup", function() {
    isDragging = false;
    sliderListItem.classList.remove('pull');

    let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

    let translateXValue = match ? parseFloat(match[1]) : 0;
    
    let result = translateXValue/firstSliderItem.offsetWidth;

    let currentPosition = 0;
    if((result%1) >= 0) {
        currentPosition = (result%1) >= 0.5 ? Math.ceil(result) : Math.floor(result);
    } else {
        currentPosition = (result%1) >= -0.5 ? Math.ceil(result) : Math.floor(result);
    }

    let x = currentPosition * firstSliderItem.offsetWidth;

    sliderListItem.style = `transform: translateX(${x}px); transition: all 0.25s ease 0s;`;
});

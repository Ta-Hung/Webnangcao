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

if (barMenuMobile && barMenuMobile.offsetHeight == 0) {
    barMenuMobile.style.display = 'block';
    barMenuMobile.style.height = 'auto';
    heightContentMenuMobile = barMenuMobile.offsetHeight;
    barMenuMobile.style.display = 'none';
    barMenuMobile.style.height = '0px';
} else if (barMenuMobile) {
    heightContentMenuMobile = barMenuMobile.offsetHeight;
}

if (barBrandsMobile && barBrandsMobile.offsetHeight == 0) {
    barBrandsMobile.style.display = 'block';
    barBrandsMobile.style.height = 'auto';
    heightContentBrandsMobile = barBrandsMobile.offsetHeight;
    barBrandsMobile.style.display = 'none';
    barBrandsMobile.style.height = '0px';
} else if (barBrandsMobile){
    heightContentBrandsMobile = barBrandsMobile.offsetHeight;
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

if (barTitleMenuMobile) {
    barTitleMenuMobile.addEventListener('click', () => {
        if (enableMenuSidebarClick) {
            if (barMenuMobile.offsetHeight == 0) {
                enableMenuSidebarClick = false;
                iconBarTitleMenuMobile.classList.toggle('rotate180');
                HandleHeight('.container__sidebar-menu-content', 16.67, 300, heightContentMenuMobile);
            } else if (barMenuMobile.offsetHeight == heightContentMenuMobile) {
                enableMenuSidebarClick = false;
                iconBarTitleMenuMobile.classList.toggle('rotate180');
                HandleHeight('.container__sidebar-menu-content', 16.67, 300, heightContentMenuMobile);
            }
        }
    })

    barTitleBrandsMobile.addEventListener('click', () => {
        if (enableMenuSidebarClick) {
            if (barBrandsMobile.offsetHeight == 0) {
                enableMenuSidebarClick = false;
                iconBarTitleBrandsMobile.classList.toggle('rotate180');
                HandleHeight('.container__sidebar-brands-content', 16.67, 300, heightContentBrandsMobile);
            } else if (barBrandsMobile.offsetHeight == heightContentBrandsMobile) {
                enableMenuSidebarClick = false;
                iconBarTitleBrandsMobile.classList.toggle('rotate180');
                HandleHeight('.container__sidebar-brands-content', 16.67, 300, heightContentBrandsMobile);
            }
        }
    })
}


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
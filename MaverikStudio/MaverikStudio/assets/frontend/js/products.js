let productDetailThumbs = document.querySelectorAll('.products__detail-thumb');
let productDetailThumbTemp = productDetailThumbs[0];
let productDetailImagePc = document.querySelector('.products__detail-image-pc img');

for(let i=0;i<productDetailThumbs.length;i++) {
    productDetailThumbs[i].addEventListener('click', () => {
        if(productDetailThumbTemp) {
            productDetailThumbTemp.classList.remove('active');
        }

        productDetailThumbs[i].classList.add('active');
        productDetailThumbTemp = productDetailThumbs[i];

        let mainImagePc = productDetailThumbs[i].getAttribute('url-image-pc');
        productDetailImagePc.src = mainImagePc;
    });
}

let showRoom = document.querySelector('.show-room');
let iconRoom = document.querySelector('.icon-zoom');
let iconRoomCLose = document.querySelector('.show-room__close');

iconRoom.addEventListener('click', () => {
    idBody.classList.add('locked-scroll');
    showRoom.classList.add('show');
});

iconRoomCLose.addEventListener('click', () => {
    idBody.classList.remove('locked-scroll');
    showRoom.classList.remove('show');
});
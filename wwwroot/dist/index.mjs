function listenResize(helper) {
    helper.invokeMethodAsync('SetSize', window.innerWidth, window.innerHeight);
    window.addEventListener('resize', () => {
        helper.invokeMethodAsync('OnResize', window.innerWidth, window.innerHeight);
    });
}
window.listenResize = listenResize;

export { listenResize as default };

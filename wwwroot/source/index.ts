export default function listenResize(helper: any) {
    helper.invokeMethodAsync('SetSize', window.innerWidth, window.innerHeight)
    window.addEventListener('resize', () => {
        helper.invokeMethodAsync('OnResize', window.innerWidth, window.innerHeight)
    })
}

declare global {
    interface Window {
        listenResize: typeof listenResize
    }
}

window.listenResize = listenResize
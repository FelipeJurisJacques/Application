export default function listenResize(helper: any) {
    helper.invokeMethodAsync('OnResize', screen.width, screen.height)
    window.addEventListener('resize', () => {
        helper.invokeMethodAsync('OnResize', screen.width, screen.height)
    })
}

declare global {
    interface Window {
        listenResize: typeof listenResize
    }
}

window.listenResize = listenResize
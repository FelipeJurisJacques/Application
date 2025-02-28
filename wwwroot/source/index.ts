import { Agent } from "./helpers/Agent"
// @ts-ignore
import * as THREE from "./libs/three/three.module.js"

const agent = new Agent()
let renderer: null | THREE.WebGLRendere = null
let camera: null | THREE.PerspectiveCamera = null

function animate(): void {
    if (renderer) {
        agent.animate()
        renderer.render(agent.Scene, camera)
    }
    requestAnimationFrame(animate)
}

export function listenResize(helper: any) {
    try {
        helper.invokeMethodAsync('SetSize', window.innerWidth, window.innerHeight)
        window.addEventListener('resize', () => {
            helper.invokeMethodAsync('OnResize', window.innerWidth, window.innerHeight)
            if (renderer) {
                camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
                camera.position.set(0, -2, 0)
                camera.lookAt(new THREE.Vector3(0, 0, 0))
                renderer.setSize(window.innerWidth, window.innerHeight)
            }
        })
    } catch (error) {
        console.error(error)
        throw error
    }
}

export function initializeAgent(helper: any): Agent {
    try {
        renderer = new THREE.WebGLRenderer({
            canvas: document.body.querySelector('#background') as HTMLCanvasElement,
            antialias: true
        })
        camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
        camera.position.set(0, -2, 0)
        camera.lookAt(new THREE.Vector3(0, 0, 0))
        renderer.setSize(window.innerWidth, window.innerHeight)
        animate()
        return agent
    } catch (error) {
        console.error(error)
        throw error
    }
}

declare global {
    interface Window {
        agent: Agent
        listenResize: typeof listenResize
        initializeAgent: typeof initializeAgent
    }
}

window.agent = agent
window.listenResize = listenResize
window.initializeAgent = initializeAgent
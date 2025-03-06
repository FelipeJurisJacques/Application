// @ts-ignore
import * as THREE from "../libs/three/three.module.js"

export class Agent {
    private n: number
    private l1: number
    private l2: number
    private ray: number
    private left: number
    private right: number
    private delta: number
    private scene: THREE.Scene
    private offset: number
    private intensity: number
    private points_depth: number
    private points_length: number
    private points_geometry: THREE.BufferGeometry
    private ring_transforms: THREE.RingGeometry[]
    
    public constructor() {
        this.ray = 0.5
        const length = Math.round(512 * this.ray)
        this.n = 1
        this.l1 = length / 5
        this.l2 = length / 7
        this.left = 0
        this.right = 0
        this.delta = 0.0
        this.scene = new THREE.Scene()
        this.offset = 0.0
        this.intensity = 0.0
        this.points_depth = Math.round(16 * this.ray)
        this.points_length = length
        this.ring_transforms = []

        const gridHelper = new THREE.GridHelper(10, 10)
        this.scene.add(gridHelper)

        const ambientLight = new THREE.AmbientLight(0xffffff, 5)
        this.scene.add(ambientLight)

        const directionalLight1 = new THREE.DirectionalLight(0xffffff, 5)
        directionalLight1.position.set(1, 0, 1)
        this.scene.add(directionalLight1)
        const directionalLight2 = new THREE.DirectionalLight(0xffffff, 5)
        directionalLight2.position.set(-1, 0, -1)
        this.scene.add(directionalLight2)

        const geometry = new THREE.BufferGeometry()
        const material = new THREE.PointsMaterial({
            color: 0xffffff,
            size: 0.001 + Math.random() * 0.003,
            sizeAttenuation: true
        })
        const vertices = []
        for (let i = 0; i < this.points_length * this.points_depth; i++) {
            vertices.push(0.0, 0.0, 0.0)
        }
        geometry.setAttribute('position', new THREE.Float32BufferAttribute(vertices, 3))
        const points = new THREE.Points(geometry, material)
        this.scene.add(points)
        this.points_geometry = geometry

        const ring_geometry = new THREE.RingGeometry(
            0.3,
            0.4,
            32,
            1,
            Math.PI / 2,
            Math.PI
        )
        const ring_material = new THREE.MeshBasicMaterial({
            color: 0x00ff00,
            side: THREE.DoubleSide
        })
        const ring = new THREE.Mesh(ring_geometry, ring_material)
        this.ring_transforms.push(ring)
        this.scene.add(ring)
    }

    public get Scene(): THREE.Scene {
        return this.scene
    }

    public animate() {
        this.animateWave()
        this.delta += (this.intensity - this.delta) * 0.2
    }

    public speak(message: string) {
        const speak = new SpeechSynthesisUtterance(message)
        speak.rate = 2
        speak.pitch = 1
        speak.volume = 1
        window.speechSynthesis.speak(speak)
        this.speaking(
            message.toLocaleLowerCase(),
            0,
            100 / speak.rate,
        )
    }

    private speaking(message: string, index: number, rate:number)
    {
        let time = rate
        const char = message.at(index)
        if (char === ' ') {
            this.intensity = 0.0
        } else if (
            char === '.'
            || char === '?'
            || char === '!'
            || char === ','
            || char === ';'
            || char === ':'
        ) {
            time *= 2
            this.intensity = 0.0
        } else if (
            char === 'a'
            || char === 'e'
            || char === 'i'
            || char === 'o'
            || char === 'u'
        ) {
            this.intensity = 1.0
        } else {
            this.intensity = 0.5
        }
        if (index > message.length) {
            this.intensity = 0.0
        } else {
            setTimeout(() => {
                this.speaking(message, index + 1, rate)
            }, time)
        }
    }

    private vertical(position: any, indexes: number[], angle: number) {
        const vertex = new THREE.Vector3()
        let i = 0
        for (let index of indexes) {
            const offset = this.wave(i++)
            vertex.fromBufferAttribute(position, index)
            vertex.set(
                Math.cos(angle) * offset * this.ray,
                vertex.y,
                Math.sin(angle) * offset * this.ray,
            )
            position.setXYZ(index, vertex.x, vertex.y, vertex.z)
        }
    }

    private animateWave() {
        if (this.delta > 0.1) {
            this.left -= this.delta * 2.0
            this.right -= this.delta * 0.4
        } else {
            this.left -= 0.1
            this.right -= 0.002
        }
        let angle = 0.0
        let indexes = []
        this.offset += 0.05
        const delta = Math.PI * 2.0 / this.points_length
        const position = this.points_geometry.attributes.position
        const length = position.count
        for (let i = 0; i < length; i++) {
            if (i % this.points_depth === 0) {
                this.vertical(position, indexes, angle)
                angle += delta
                indexes = []
                this.left -= 1.0
                this.right += 1.0
            }
            indexes.push(i)
        }
        position.needsUpdate = true
    }

    private wave(y: number) {
        const angle = Math.PI / 2
        const external = this.fourierSeries(this.left, this.l1)
        const internal = this.fourierSeries(this.right, this.l2)
        let balance = y / this.points_depth
        let sin = Math.sin(balance * angle)
        let cos = Math.sin((1.0 - balance) * angle)
        let value = external * sin + internal * cos
        value += 1.0
        return value
    }

    private fourierSeries(x: number, l: number) {
        const amp = 0.1 + this.delta * 0.1
        if (x % this.l1 === 0) {
            this.n = Math.floor(Math.random() * 3) + 1
        }
        let sum = 0
        for (let n = 1; n <= this.n; n++) {
            sum += Math.sin(2 * Math.PI * n * x / l) / n
        }
        return sum * amp + amp * this.n
    }
}
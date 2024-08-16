const width = window.innerWidth
const height = window.innerHeight

// init

const camera = new THREE.PerspectiveCamera(70, width / height, 1, 1000)
camera.position.z = 10

const scene = new THREE.Scene()

//const geometry = new THREE.BoxGeometry(0.2, 0.2, 0.2);
//const material = new THREE.MeshNormalMaterial();
//const mesh = new THREE.Mesh(geometry, material);

const geometry = new THREE.RingGeometry(4, 5, 64)
const material = new THREE.MeshBasicMaterial({ color: 0xffff00, side: THREE.DoubleSide })
const mesh = new THREE.Mesh(geometry, material)

scene.add(mesh)

const renderer = new THREE.WebGLRenderer({ antialias: true })
renderer.setSize(width, height)
renderer.setAnimationLoop(animate)
document.body.appendChild(renderer.domElement)

// animation

function animate(time) {
	// requestAnimationFrame(animate)
	mesh.rotation.x = time / 2000
	mesh.rotation.y = time / 1000
	// const geometry = mesh.geometry
	// geometry.parameters.outerRadius = Math.random() + 5
	// geometry.verticesNeedUpdate = true
	renderer.render(scene, camera)
}
# Descrición

Version multijugador de CrazyZombie que permite enfrentarse tanto a los zombies como a otros jugadores.

# Título principal

**NetCrazyZombie**

## Subtítulo

Se han hecho los siguientes cambios respecto al proyecto original:

- Se han modificado el *Rigidbody* tanto del jugador como de los zombies, congelandoles la rotacion y aumentandoles la masa para evitar empujes raros al colisionar.

- Se ha hecho el salto del jugador independiente de la masa del *Rigidbody*.

- Se ha dividido el prefab **Bullet** en 3 prefabs: **BulletBase**, **BulletClient** y **BulletServer**. 

- - **BulletBase** contiene los scripts *LifeTime.cs* y *BulletMove* en el propio prefab y  *DestroySelfOnContact* junto a un *Rigidbody* y un *Capsule Collider* en un elemento hijo.

- - **BulletClient** incorpora a mayores un *AudioSource* en el propio prefab y un script *BulletHit.cs* y un *MeshRenderer* en el hijo.

- - **BulletServer** incorpora a mayores un script *DealDamageOnContact.cs* en el hijo.

- Se ha modificado el script *BulletMove.cs* para que solo se encargue del movimiento de la bala, trasladandose las funciones de hacer daño y destruirse pasado x tiempo a los scripts *DealDamageOnContact.cs* y *LifeTime.cs* respectivamente

- Se ha modificado el sistema de salud y de visualización de la misma. 
- - En vez de estar en el *PlayerManager.cs* ahora la salud esta en su propio script *PlayerHealth.cs* el cual contiene una serie de metodos que pueden ser llamados para restar, sumar y reiniciar la vida del jugador.
- - Ahora dentro de **PlayerDataUI** en el prefab **Player** existe un script *PlayerHealthDisplay.cs* que se encarga de detectar cuando cambia la vida del jugador para reflejarlo en su barra de vida.

- Se ha modificado el sistema de respawn. Ahora existen 6 puntos fijos de respawn en el mapa dentro de **PlayerSpawner** los cuales son seleccionados aleatoriamente mediante el script *SpawnPointManager.cs*. El script busca el punto valido mas cercano al punto de spawn seleccionado y instancia ahí al jugador. En caso de que fuese imposible instanciarlo, el jugador spawneará en el punto de spawn que se empleaba por defecto anteriormente.
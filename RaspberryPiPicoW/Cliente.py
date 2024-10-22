import network
import socket
import time
from machine import Pin, Timer

# Configuración del LED y el botón
led = Pin(15, Pin.OUT)
button = Pin(14, Pin.IN, Pin.PULL_DOWN)
ledButton = Pin(16, Pin.OUT)

# Variables del botón
tiempoPresionado = 0
botonPresionado = False
tiempoDeInicio = 0
cargaMax = 3  # segundos para el 100% de velocidad

# Configuración de la red
ssid = 'FAMILIA CB'
password = '13638721'

# Conexión a la red Wi-Fi
wlan = network.WLAN(network.STA_IF)
wlan.active(True)
wlan.connect(ssid, password)

while not wlan.isconnected():
    time.sleep(0.1)

print('Conectado a la red:', wlan.ifconfig())

# Configuración del socket (Servidor)
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind(('0.0.0.0', 12345))  # IP de la Pico W y puerto
s.listen(1)

conn, addr = s.accept()
print('Conexión desde:', addr)

# Configuración del temporizador
timer = Timer()

def apagar_led(timer):
    led.off()

while True:
    # Recepción de mensajes del programa MAUI
    mensaje = conn.recv(1024).decode()
    if mensaje == 'DisparoMau':
        print('Recibido: DisparoMau')
        led.on()
        # Configurar el temporizador para apagar el LED después de 2 segundos
        timer.init(mode=Timer.ONE_SHOT, period=2000, callback=apagar_led)

    # Envío de mensajes al programa MAUI si se presiona el botón
    if button.value() == 1:
        if not botonPresionado:
            # Si el botón acaba de ser presionado, inicia el contador
            tiempoDeInicio = time.ticks_ms()
            botonPresionado = True
            ledButton.on()
    else:
        if botonPresionado:
            # Si el botón estaba presionado y se ha soltado ahora
            duracion = (time.ticks_ms() - tiempoDeInicio) / 1000
            if duracion > cargaMax:
                duracionPorcentage = 100
            else:
                duracionPorcentage = str(int(duracion / cargaMax * 100))
            
            # Envío del mensaje al programa MAUI, string del 1 al 100 (representa porcentaje)
            conn.sendall(duracionPorcentage.encode())
            
            # Reiniciar las variables de estado
            botonPresionado = False
            ledButton.off()
            
    time.sleep(0.009)  # Pequeño retardo para evitar sobrecargar la CPU

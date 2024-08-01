import asyncio
import websockets
import sys

async def communicate_with_server(cmd):
    uri = "ws://10.79.85.90:8080/Server" # quest: 10.79.85.90 # win: 10.79.44.94
    # uri = "ws://10.79.44.94:8080/Server"
    async with websockets.connect(uri) as websocket:
        await websocket.send(cmd)
        response = await websocket.recv()
        print(f"{response}")
        
# print()

asyncio.get_event_loop().run_until_complete(communicate_with_server(sys.argv[1]))
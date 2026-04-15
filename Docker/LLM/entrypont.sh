#!/bin/sh

ollama serve &

# espera até o ollama responder
until curl -s http://localhost:11434 > /dev/null; do
  sleep 1
done

ollama pull qwen3:8b

wait

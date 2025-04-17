# 🚀 Feature Toggles com LaunchDarkly + Relay Proxy

Este projeto é uma **Prova de Conceito (PoC)** que demonstra como integrar o [LaunchDarkly](https://launchdarkly.com/) com uma API .NET utilizando o **modo daemon** via **Relay Proxy**.



## 🐳 Iniciando o projeto

### Pré-requisitos

- [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/)

### Subindo a aplicação

1. **Inicie o Docker**
2. Execute o projeto `POC.LaunchDarkly.Api.AppHost`

> ℹ️ O Aspire se encarregará de subir os projetos dependentes definidos na solução.


## 🧪 Testando a PoC

A pasta [`/docs`](./docs) contém a coleção Postman com os seguintes endpoints:

**Relay Proxy**
- `GET http://localhost:8030/status` → Verifica o status do proxy  
- `GET http://localhost:8030/flags` → Lista todas as feature flags

**API**
- `GET /api/v1/emprestimos/simulacao` → Simula um empréstimo  
- `POST /api/v1/emprestimos` → Cria um empréstimo

### 🌐 Gerenciar Feature Flags

Acesse o portal do LaunchDarkly para visualizar e gerenciar as flags:

🔗 [https://app.launchdarkly.com](https://app.launchdarkly.com)  
👤 **Usuário:** `seu-usuario@exemplo.com`  
🔑 **Senha:** `sua-senha`


## 🧠 Arquitetura: Relay Proxy em Modo Daemon

### Cenário de uso

Nesta PoC, optamos pelo uso do **Relay Proxy em modo daemon** para otimizar a comunicação entre os serviços backend e o LaunchDarkly.

Temos múltiplos serviços backend que precisam consultar feature flags. Conectar todos diretamente ao LaunchDarkly gera:

- Múltiplas conexões simultâneas
- Maior tráfego de rede
- Redundância no acesso ao banco de dados

### Solução com Relay Proxy

Com o Relay Proxy em modo daemon:

- O **Relay Proxy** mantém a **única conexão** ativa com o LaunchDarkly, recebendo atualizações em tempo real
- As atualizações são armazenadas no **Redis**
- Os serviços backend consultam apenas o Redis
- Não há necessidade de comunicação externa dos serviços

<p >
  <img src="docs/architecture/relay-daemon.png" alt="Arquitetura Relay Proxy em modo daemon" width="600"/>
</p>

> 💡 **Importante**: Esta arquitetura é voltada exclusivamente para **SDKs de back-end (server-side)**.  
> SDKs client-side **devem se comunicar diretamente com o LaunchDarkly**, pois dependem de conexões individuais e atualizações contínuas.


## 🔗 Links úteis

- 📘 [Documentação oficial do LaunchDarkly](https://launchdarkly.com/docs/)
- 🧠 [Relay Proxy](https://launchdarkly.com/docs/sdk/relay-proxy)
- 🐳 [Docker Relay Proxy - GitHub](https://github.com/launchdarkly/ld-relay/blob/master/README.md)
- ⚙️ [Configurando SDK com Daemon Mode](https://launchdarkly.com/docs/sdk/features/relay-proxy-configuration/daemon-mode)
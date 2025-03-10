# Assistente de Manutenção Preventiva

### O Assistente de Manutenção Preventiva é uma ferramenta desenvolvida para auxiliar na manutenção e otimização do Windows, permitindo realizar diversas tarefas automáticas, como limpeza de arquivos temporários, desativação de processos e backup de configurações importantes.

#

![Preview](https://github.com/user-attachments/assets/8eb0515e-4ec8-4f08-b660-987e0dcec43a)


#

### Funcionalidades
- [x] Controle de Conta de Usuário (UAC)
- [x] Limpar Agenda de tarefas do Windows
- [x] Limpar Lixeira
- [x] Limpar Processos
- [x] Remover arquivos temporários (%Temp%)
- [x] Remover arquivos Update Windows
- [x] Remover arquivos Google Update
- [x] Backup Registro do Windows
- [x] Gerenciamento do arquivo Pagefile.sys
- [x] Backup de Drivers
- [x] Limpar Registros do Windows
- [x] Cria usuário "TerraVista"
- [x] Limpar Prefetch
- [x] Backup do BootBCD
- [ ] Desativar Windows Update 
- [x] Salva Log dos processos executados 

### Requisitos
- Windows 10 ou superior
- Permissões de administrador para executar

### Clone este repositório:
``` bash
git clone https://github.com/DanielCampos2017/MeuSuporte.git
```

### Contribuição
#### Sinta-se à vontade para sugerir melhorias ou relatar problemas abrindo uma issue no repositório.
# 
### Observação das funcionalidade

##### Usuário "terravista"
 - Cria usuário local caso não exista
 - Edita usuário, atualizar a senha e coloca no grupo administrador da máquina.

##### Serviços do Windows que são apagados
``` bash
# Serviço da Adobe
  - AdobeUpdateService
  - AGSService
  - AdobeARMservice
  - serviço da adobe

# Microsoft Office Groove Audit Service
  - serviço do Office

# serviço do google
  - edgeupdate 
  - edgeupdatem 
  - gupdatem
  - gupdate
  - GoogleChromeElevationService
  - gusvc

# Dropbox Service  
  - DbxSvc

# Pode ajudar em SSDs, mas em HDs pode ser útil
  - SysMain

# Atualizar o Serviço Orchestrator 
  - UsoSvc

# Xbox Accessory Management Service   
  - XboxGipSvc

# Serviço Direto do Hyper-V PowerShell
  - vmicvmsession

# Serviço de Virtualização de Área de Trabalho Remota do Hyper-V 
  - vmicrdv

# Serviço de User Experience Virtualization
  - UevAgentService

# Serviço de Troca de Dados do Hyper-V
  - vmickvpexchange

# Serviço de Telefonia
  - PhoneSvc

# Serviço de Sincronização de Data/Hora do Hyper-V
  - vmictimesync

# Serviço de Simulação de Percepção do Windows
  - perceptionsimulation

# Serviço de Sensor rotação automática
  - SensorService

# Serviço de Rede Xbox Live
  - XboxNetApiSvc

# Serviço de Pulsação do Hyper-V 
  - vmicheartbeat

# Serviço de Host HV
  - HvHost

# Serviço de identidade Microsoft Cloud 
  - cloudidsvc

# Serviço de Hotspot Móvel do Windows 
  - icssvc

# Serviço de Histórico de Arquivos 
  - fhsvc

# Serviço de Geolocalização 
  - lfsvc

# Serviço de duplicação do ReFS 
  - refsdedupsvc

# Serviço de Desligamento de Convidado do Hyper-V 
  - vmicshutdown

# Serviço de Compartilhamento de Rede do Windows Media Player 
  - WMPNetworkSvc

# Serviço de Biometria do Windows 
  - WbioSrvc

# Se você não usa fax, pode desativar.
  - Fax

# Serviço de Gerenciador de NFC/SE e Pagamentos           
  - SEMgrSvc

# Serviço de Mapas
  - MapsBroker

# Serviço de Notificações Push do Windows
  - WpnService

# Serviço de Autenticação Xbox Live
  - XblAuthManager

```
# 
##### Serviços do Windows que são Desativado

``` bash
# Serviço de Instalação da Microsoft Store
  - InstallService

# Serviço de Verificador de Ponto
  - svsvc

# Serviço de Windows Update
  - wuauserv

# Pesquisa do Windows
  - WSearch

```
# 

### Limpeza de Registros
``` bash
# Chaves do Registro 
  Computador\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run

  Computador\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
```

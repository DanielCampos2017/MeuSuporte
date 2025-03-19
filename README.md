# Assistente de Manutenção Preventiva

#### O Assistente de Manutenção Preventiva é uma ferramenta desenvolvida para auxiliar na manutenção e otimização do Windows, permitindo realizar diversas tarefas automáticas, como limpeza de arquivos temporários, desativação de processos e backup de configurações importantes.

#

![Preview](https://github.com/user-attachments/assets/81130f2a-96de-4cef-987c-2047968d07b0)

#

### Funcionalidades
- [x] Controle de Conta de Usuário (UAC)
- [x] Limpar Agenda de tarefas do Windows
- [x] Limpar Lixeira
- [x] Limpar Processos
- [x] Remover arquivos temporários (%Temp%)
- [x] Desativar Windows Update
- [x] Remover arquivos Google Update
- [x] Backup Registro do Windows
- [x] Gerenciamento do arquivo Pagefile.sys
- [x] Backup de Drivers
- [x] Limpar Registros do Windows
- [x] Cria usuário "TerraVista"
- [x] Limpar Prefetch
- [x] Backup do BootBCD 
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

### 🔧 Lista de Serviços do Windows que são apagados

<details>
  <summary>🔽 Clique para expandir</summary>

#### **Serviço da Adobe**
- AdobeUpdateService
- AGSService
- AdobeARMservice

#### **Microsoft Office Groove Audit Service**
- serviço do Office

#### **Serviço do Google**
- edgeupdate 
- edgeupdatem 
- gupdatem
- gupdate
- GoogleChromeElevationService
- gusvc

#### **Dropbox Service**
- DbxSvc

#### **Serviço relacionado a desempenho**
- SysMain //Pode ajudar em SSDs, mas em HDs pode ser útil)

#### **Outros Serviços**
- UsoSvc - Atualização do Serviço Orchestrator)
- XboxGipSvc - Xbox Accessory Management Service)
- vmicvmsession - Serviço Direto do Hyper-V PowerShell
- vmicrdv - Serviço de Virtualização de Área de Trabalho Remota do Hyper-V)
- UevAgentService - User Experience Virtualization)
- vmickvpexchange - Serviço de Troca de Dados do Hyper-V)
- PhoneSvc - Serviço de Telefonia)
- vmictimesync - Serviço de Sincronização de Data/Hora do Hyper-V)
- perceptionsimulation - Serviço de Simulação de Percepção do Windows)
- SensorService - Serviço de Sensor rotação automática)
- XboxNetApiSvc - Serviço de Rede Xbox Live)
- vmicheartbeat - Serviço de Pulsação do Hyper-V)
- HvHost - Serviço de Host HV)
- cloudidsvc - Serviço de identidade Microsoft Cloud)
- icssvc - Serviço de Hotspot Móvel do Windows)
- fhsvc - Serviço de Histórico de Arquivos)
- lfsvc - Serviço de Geolocalização)
- refsdedupsvc - Serviço de duplicação do ReFS)
- vmicshutdown - Serviço de Desligamento de Convidado do Hyper-V)
- WMPNetworkSvc - Serviço de Compartilhamento de Rede do Windows Media Player)
- WbioSrvc - Serviço de Biometria do Windows)
- Fax - Se você não usa fax, pode desativar)
- SEMgrSvc - Serviço de Gerenciador de NFC/SE e Pagamentos)
- MapsBroker - Serviço de Mapas)
- WpnService - Serviço de Notificações Push do Windows)
- XblAuthManager - Serviço de Autenticação Xbox Live)

</details>

# 

### 🔧 Desativar Windows Update

<details>
  <summary>🔽 Clique para expandir</summary>
 
  #### **Serviço que serão desativados**
- InstallService - Serviço de Instalação da Microsoft Store
- svsvc - Serviço de Verificador de Ponto
- wuauserv - Serviço de Windows Update
- WSearch - Pesquisa do Windows
  
</details>

### Limpeza de Registros
``` bash
# Chaves do Registro 
  Computador\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
  Computador\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
  Computador\HKEY_LOCAL_MACHINE\Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run
```

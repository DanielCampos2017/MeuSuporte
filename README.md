# Assistente de Manutenção Preventiva

#### O Assistente de Manutenção Preventiva é uma ferramenta desenvolvida para auxiliar na manutenção e otimização do Windows, permitindo realizar diversas tarefas automáticas, como limpeza de arquivos temporários, desativação de processos e backup de configurações importantes.

#

![Screenshot_2](https://github.com/user-attachments/assets/350449fe-8cf7-4e8a-ab22-8d0138e44d46)


#

### Funcionalidades
- [x] Notificação UAC ao Usuário
- [x] Limpar tarefas Agendadas
- [x] Limpar Lixeira
- [x] Desativar Processos
- [x] Limpar Pasta %Temp%
- [x] Desativar o Windows Update
- [x] Limpar Google Update
- [x] Backup Registro
- [x] Pagefile.sys
- [x] Backup de Driver
- [x] Limpar Registros
- [x] Usuário "Suporte"
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

### Documentação das funcionalidade do programa

<details>
  <summary>🔽 Limpar Processos</summary>
 
``` bash
#   Serviço da Adobe
- AdobeUpdateService
- AGSService
- AdobeARMservice

#   Microsoft Office Groove Audit Service
- serviço do Office

#   Serviço do Google
- edgeupdate 
- edgeupdatem 
- gupdatem
- gupdate
- GoogleChromeElevationService
- gusvc

#   Dropbox Service
- DbxSvc

#   Serviço relacionado a desempenho
- SysMain //Pode ajudar em SSDs, mas em HDs pode ser útil)

#   Outros Serviços
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
 ```
</details>




<details>
  <summary>🔽 Backup Registro</summary>

 ``` bash
#   Cria um Backup das chave de registro dos diretorios
  Computador\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
  Computador\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
  Computador\HKEY_LOCAL_MACHINE\Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run 

 ``` 
</details>

<details>
  <summary>🔽 Pagefile.sys</summary>

 ``` bash
#   Ativa\Desativa Paginação do Windows 
#   Alterar valor da chave de registro
   ClearPageFileAtShutdown
#   Local
   SYSTEM\CurrentControlSet\Control\Session Manager\ClearPageFileAtShutdown
 ``` 
</details>

<details>
 <summary>🔽 Backup de Drivers </summary>
 
 ``` bash
#   Cria um Backup de todos os Drivers do computador
#   O processo de criação dos Backup é semelhante a execução do comando no cmd "dism /online /export-driver /destination:C:\DriversBackup"
#   Para restaura basta executar o comando no cmd como administrador comando: pnputil /add-driver "C:\DriversBackup\*.inf" /subdirs /install
 ```
</details>

<details>
  <summary>🔽 Desativar o Windows Update</summary>

 ``` bash
#   Todos os Serviços abaixo sera desativado
- InstallService - Serviço de Instalação da Microsoft Store
- svsvc - Serviço de Verificador de Ponto
- wuauserv - Serviço de Windows Update
- WSearch - Pesquisa do Windows
 ``` 
</details>

<details>
 <summary>🔽 Limpar Google Update</summary>
 
``` bash
#   Limpa a pasta de Update do Google Chrome   
C:\Program Files (x86)\Google\Update
```
</details>

<details>
 <summary>🔽 Limpar Registros</summary>
 
``` bash
#   Todas as chaves do Registro nesses diretório sera apagados
  Computador\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
  Computador\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
  Computador\HKEY_LOCAL_MACHINE\Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run
```
</details>

<details>

 <summary>🔽 Usuário "Suporte"</summary>
 
``` bash

#   Cria o Usuario chamado "Suporte" e adiciona no grupo de adiministrador
#   Caso exista o usuario "Suporte atualiza a senha e coloca no grupo administrador
```
</details>

<details>
 <summary>🔽 Limpar Prefetch</summary>
 
 ``` bash
#   Limpa a pasta do diretorio C:\Windows\Prefetch
 ```
</details>

<details>
 <summary>🔽 Backup do BootBCD </summary>
 
 ``` bash
#   Faz cópia de segurança da configuração de inicialização do Windows para recupera falhas no boot
 ```
</details>

<details>
 <summary>🔽 Salva Log dos processos executados </summary>
 
 ``` bash
#   Ao tentar fechar o programa ele cria um log dos processos realizados. O log é salvo no mesmo diretorio de execução do programa
 ```
</details>

﻿using System;
using System.Reflection;
using MeuSuporte.Properties;

namespace MeuSuporte
{
    internal class CheckBuild_Process
    {

        public void Mensage(Version BuildVersion, int BuildResult, string GitHubRepo)
        {
            string BuildLocal = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";

            // verifica o resultado da versão do GiHub
            if (BuildVersion == new Version(0, 0, 0, 0))
            {
                // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                WinGlobal_UIService.Instance.InterfaceGUI.Invoke(new Action(() =>
                {
                    WinGlobal_UIService.Instance.InterfaceGUI.Text = $"MeuSuporte Build {BuildLocal} - Unknown";  // Versão não encontrada no GitHub
                    WinGlobal_UIService.Instance.InterfaceGUI.PainelInfoDescricao(Resources.Alert_Black, $"404 Not Found!:\r\nNão foi possível encontrar repositório. https://github.com/{GitHubRepo}/releases/latest");
                }));

                return;
            }


            if (BuildResult > 0)
            {
                // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                WinGlobal_UIService.Instance.InterfaceGUI.Invoke(new Action(() =>
                {
                    WinGlobal_UIService.Instance.InterfaceGUI.Text = $"MeuSuporte Build {BuildVersion} - Beta"; // Versão local maior que a do GitHub
                    WinGlobal_UIService.Instance.InterfaceGUI.PainelInfoDescricao(Resources.Code_Black, $"Verão Beta\r\nEssa é uma versão de desenvolvimento. Que ainda se encontra em desenvolvimento e não trata de uma versão final !");
                }));
            }
            else if (BuildResult < 0)
            {
                // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                WinGlobal_UIService.Instance.InterfaceGUI.Invoke(new Action(() =>
                {
                    WinGlobal_UIService.Instance.InterfaceGUI.checkBoxAllState(false);
                    WinGlobal_UIService.Instance.InterfaceGUI.Text = $"MeuSuporte Build {BuildVersion} - Legacy"; // Versão local inferior à do GitHub                       
                    WinGlobal_UIService.Instance.InterfaceGUI.PainelInfoDescricao(Resources.Dawnload_Black, $"Atualização Disponível!:\r\nPara garantir o melhor eficiência baixe últimas versão. https://github.com/{GitHubRepo}/releases/latest");
                }));
            }
            else
            {
                // Usando Invoke para atualizar a interface do usuário a partir da thread de segundo plano
                WinGlobal_UIService.Instance.InterfaceGUI.Invoke(new Action(() =>
                {
                    WinGlobal_UIService.Instance.InterfaceGUI.Text = $"MeuSuporte Build {BuildVersion} - Stable"; // Versão local igual à do GitHub
                    WinGlobal_UIService.Instance.InterfaceGUI.PainelInfoDescricao(Resources.Security_Black, $"Estável:\r\nSua verão se encontrar na mesma versão do repositório. https://github.com/{GitHubRepo}/releases/latest");
                }));
            }
        }
    }
}

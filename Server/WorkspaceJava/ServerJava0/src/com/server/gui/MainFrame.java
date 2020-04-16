package com.server.gui;

import java.awt.Container;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.nio.ByteBuffer;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JPanel;

import com.server.nio.Client;
import com.server.nio.ServerAsync;
import com.server.protocol.ProtocolObject;
import com.server.protocol.Protocol.EnumKey;
import com.server.protocol.Protocol.EnumResResult;

public class MainFrame extends JFrame {

	private static final long serialVersionUID = 1L;

	public MainFrame() {
		super("Server");

		setBounds(100 , 100 , 300 , 200);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		Container contentPane = this.getContentPane();
		JPanel pane = new JPanel();
		JButton buttonWriteAll = new JButton("Write All");
		buttonWriteAll.setMnemonic('S');

		pane.add(buttonWriteAll);
		contentPane.add(pane);

		setVisible(true);
		
		buttonWriteAll.addActionListener(new ActionListener(){
			@Override
			public void actionPerformed(ActionEvent e){
				ProtocolObject._resMessage.key = EnumKey.resMessage;
				ProtocolObject._resMessage.result = EnumResResult.success;
				ProtocolObject._resMessage.message = "전체응답";

				ByteBuffer buffer = ByteBuffer.wrap(Client._json.objectToBytes(ProtocolObject._resMessage));
				
				ServerAsync.writeAll(buffer);
			}
		});
	}
}


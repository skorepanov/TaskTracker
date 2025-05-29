import React from "react";
import { Link, useLocation } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Menu } from "antd";
import { useStore } from "../stores/RootStore";

const MainMenu: React.FC = observer(() => {
    const location = useLocation();

    const { folderStore } = useStore();

    const folderMenuItems = folderStore.folders.map(f => (
        <Menu.Item key={`/folders/${f.id}`}>
            <Link to={`/folders/${f.id}`}>
                [{f.id}] {f.title}
            </Link>
        </Menu.Item>
    ));

    return (
        <Menu
            mode="inline"
            selectedKeys={[location.pathname]}
            style={{ width: "270px" }}>
            <Menu.Item key="/">
                <Link to="/">Главная</Link>
            </Menu.Item>
            <Menu.Item key="/all">
                <Link to="/all">Все задачи</Link>
            </Menu.Item>
            <Menu.Item key="/today">
                <Link to="/today">Сегодня</Link>
            </Menu.Item>
            <Menu.Item key="/inbox">
                <Link to="/inbox">Inbox</Link>
            </Menu.Item>
            <Menu.Divider />
            <Menu.SubMenu title="Папки">{folderMenuItems}</Menu.SubMenu>
            <Menu.Divider />
            <Menu.Item key="/trash">
                <Link to="/trash">Корзина</Link>
            </Menu.Item>
        </Menu>
    );
});

export default MainMenu;

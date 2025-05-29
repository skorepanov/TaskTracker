import React from "react";
import { Link, useLocation } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Dropdown, Menu } from "antd";
import { useStore } from "../stores/RootStore";

const MainMenu: React.FC = observer(() => {
    const location = useLocation();

    const { taskStore, folderStore } = useStore();

    const allIncompletedTaskCount = taskStore.incompletedTasks.length;

    const todayIncompletedTaskCount =
        taskStore.getTodayIncompletedTasks().length;

    const inboxIncompletedTasksCount =
        taskStore.getInboxIncompletedTasks().length;

    const folderMenuItems = folderStore.folders.map(f => {
        const folderIncompleteTaskCount = taskStore.incompletedTasks.filter(
            t => t.folderId === f.id
        ).length;

        const handleDeleteFolderButtonClick = async () => {
            await folderStore.deleteFolder(f);
        };

        return (
            <Menu.Item key={`/folders/${f.id}`}>
                <Dropdown
                    key={f.id}
                    menu={{
                        items: [
                            {
                                label: "Удалить папку",
                                key: "deleteFolder",
                            },
                        ],
                        onClick: handleDeleteFolderButtonClick,
                    }}
                    trigger={["contextMenu"]}>
                    <Link to={`/folders/${f.id}`}>
                        <div style={{ float: "left" }}>
                            [{f.id}] {f.title}
                        </div>
                        <div style={{ float: "right", color: "grey" }}>
                            {folderIncompleteTaskCount}
                        </div>
                    </Link>
                </Dropdown>
            </Menu.Item>
        );
    });

    const getMenuItem = (
        link: string,
        title: string,
        incompletedTaskCount?: number
    ) => {
        return (
            <Menu.Item key={link}>
                <Link to={link}>
                    <div style={{ float: "left" }}>{title}</div>
                    <div style={{ float: "right", color: "grey" }}>
                        {incompletedTaskCount}
                    </div>
                </Link>
            </Menu.Item>
        );
    };

    return (
        <Menu
            mode="inline"
            selectedKeys={[location.pathname]}
            defaultOpenKeys={["/folders"]}
            style={{ width: "270px" }}>
            <Menu.Item key="/">
                <Link to="/">Главная</Link>
            </Menu.Item>
            {getMenuItem("/all", "Все задачи", allIncompletedTaskCount)}
            {getMenuItem("/today", "Сегодня", todayIncompletedTaskCount)}
            {getMenuItem("/inbox", "Inbox", inboxIncompletedTasksCount)}
            <Menu.Divider />
            <Menu.SubMenu
                key="/folders"
                title="Папки">
                {folderMenuItems}
            </Menu.SubMenu>
            <Menu.Divider />
            {getMenuItem("/trash", "Корзина")}
        </Menu>
    );
});

export default MainMenu;
